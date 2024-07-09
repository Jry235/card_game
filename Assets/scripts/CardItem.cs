using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardItem : MonoBehaviour  
{  
    /// <summary>  
    /// 卡牌扇形展开中心点  
    /// </summary>  
    public Vector3 root;  
    /// <summary>  
    /// 展开角度  
    /// </summary>  
    public float rot;  
    /// <summary>  
    /// 展开半径  
    /// </summary>  
    public float size;  
    /// <summary>  
    /// 动画速度  
    /// </summary>  
    public float animSpeed = 10;  
    /// <summary>  
    /// 高度值（决定卡牌层级）  
    /// </summary>  
    public float zPos=0;  

    bool isclicking;

    //bool isdeleting = false;

  




    public  void RefreshData(Vector3 root,float rot, float size,float zPos, bool isclicking)  
    {        
        this.root = root;  
        this.rot = rot;  
        this.size = size;  
        this.zPos = zPos;  
        this.isclicking = isclicking;
    }  





    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = (int)zPos;
        }
       
    }





    // Update is called once per frame  
    void Update()  
    {   
         
    }  







    public void SetPos()  
    {        

        //设置卡牌位置  
        float x = root.x + Mathf.Cos(rot) * size;  
        float y = root.y + Mathf.Sin(rot) * size;  
        transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, root.z), Time.deltaTime * animSpeed);  
        //设置卡牌角度  
        float rotZ = GetAngleInDegrees(root, transform.position);  
        
        Vector3 localEulerAngles = transform.localEulerAngles;  

        Quaternion rotationQuaternion;
        if (!isclicking){
            rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, rotZ));  
        }
        else{
            rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, 1));  
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationQuaternion, Time.deltaTime * animSpeed * 30);  
        
    }   







    /// <summary>  
    /// 获取两个向量之间的弧度值0-2π  
    /// </summary>    /// <param name="positionA">点A坐标</param>  
    /// <param name="positionB">点B坐标</param>  
    /// <returns></returns>    

    public static float GetAngleInDegrees(Vector3 positionA, Vector3 positionB)  
    {        
        // 计算从A指向B的向量  
        Vector3 direction = positionB - positionA;  
        // 将向量标准化  
        Vector3 normalizedDirection = direction.normalized;  
        // 计算夹角的弧度值  
        float dotProduct = Vector3.Dot(normalizedDirection, Vector3.up);  
        float angleInRadians = Mathf.Acos(dotProduct);  
  
        //判断夹角的方向：通过计算一个参考向量与两个物体之间的叉乘，可以确定夹角是顺时针还是逆时针方向。这将帮助我们将夹角的范围扩展到0到360度。  
        Vector3 cross = Vector3.Cross(normalizedDirection, Vector3.up);  
        if (cross.z > 0)  
        {            
           angleInRadians = 2 * Mathf.PI - angleInRadians;  
        }  
        // 将弧度值转换为角度值  
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;  
        return angleInDegrees;  
    }
    
    public IEnumerator Remove_Card(){
        

        //isdeleting = true;
        Vector3 destination = new Vector3(10, 25, 10);
        float closeEnough = 0.1f; // How close does the object need to be to consider it at the destination

        // Loop until the object is close enough to the destination
        while (Vector3.Distance(transform.position, destination) > closeEnough)
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * animSpeed);
            yield return null;  // Pause here, return control to Unity, then continue next frame
        }


        Destroy(gameObject);  // Destroy the GameObject
    }

    
        
    
    
    }
