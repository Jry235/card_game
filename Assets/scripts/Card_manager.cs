using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardManager : MonoBehaviour  
{  
    /// <summary>  
    /// 卡牌起始位置  
    /// </summary>  
    public Vector3 rootPos=new Vector3(0,0,0);  
    /// <summary>  
    /// 卡牌对象  
    /// </summary>  
    public GameObject cardItem;  
    /// <summary>  
    /// 扇形半径  
    /// </summary>  
    public float size =30f;  
    /// <summary>  
    /// 卡牌出现最大位置  
    /// </summary>  
    private float minPos = 1.365f;  
    /// <summary>  
    /// 卡牌出现最小位置  
    /// </summary>  
    private float maxPos = 1.73f;  
    /// <summary>  
    /// 手牌列表  
    /// </summary>  
    public List<CardItem> cardList;  
    /// <summary>  
    /// 手牌位置  
    /// </summary>  
    private List<float> rotPos;  
    /// <summary>  
    /// 最大手牌数量  
    /// </summary>  
    private int CardMaxCount= 10;  

    private Camera mainCamera;
  






    void Start()  
    {        
        InitCard();  
        mainCamera = Camera.main;
    }    
    
    
    
    
    /// <summary>  
    /// 数据初始化  
    /// </summary>  

    public  void InitCard()  
    {  
          rotPos=InitRotPos(CardMaxCount);  
    }    
  
  
  
  
    /// <summary>  
    /// 初始化位置  
    /// </summary>  
    /// <param name="count"></param>    
    /// <param name="interval"></param>    
    /// <returns></returns>    

    public List<float> InitRotPos(int count)  
    {        
        List<float> rotPos=new List<float>();  
        float interval = (maxPos - minPos)/count;  
        for (int i = 0; i < count; i++)  
        {            
            float nowPos = maxPos - interval * i;  
            rotPos.Add(nowPos);  
        }        
        return rotPos;  
    }  





    // Update is called once per frame  

    void Update()  
    {        
        TaskItemDetection();  
        RefereshCard();  

    }   





    /// <summary>  
    /// 添加卡牌  
    /// </summary>  

    public void AddCard()  
    {        
        if (cardList == null)  
        {            
            cardList = new List<CardItem>();  
        }  
        if (cardList.Count >= CardMaxCount)  
        {            
            Debug.Log("手牌数量上限");  
            return;  
        }        
        GameObject item = Instantiate(cardItem, this.transform);  
        item.transform.localPosition = new Vector3(-10, 25, 10);
        CardItem text = item.GetComponent<CardItem>();  
        text.RefreshData(rootPos, 0, 0, 0, false);  

        // 加载图片资源


        cardList.Add(text);  
    }  





    /// <summary>  
    /// 手牌状态刷新  
    /// </summary>  

    public void RefereshCard()  
    {        
        if (cardList==null)  
        {            
           return;  
        }  

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Create a ray from the mouse position
        RaycastHit hit;
        
        Collider[] childColliders = GetComponentsInChildren<Collider>();


        float start_x = rootPos.x - 0.6f * (float) Mathf.Pow((float)(cardList.Count - 1), 0.8f);
        

        for (int i = 0; i < cardList.Count; i++)  
        {   
            bool ishovering = false;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == childColliders[i]) // Check if the ray hits this object's collider
                {
                    ishovering = true;
                }
            }
    
            cardList[i].RefreshData(rootPos,rotPos[i],size,i, ishovering);
            Quaternion rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, 1));

            if (start_x != rootPos.x){

                float targetx = (start_x + i * ((rootPos.x - start_x) * 2) / (cardList.Count - 1));
                float targety = Camera.main.transform.pos;


                

                if (!ishovering){
                    rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, (rootPos.x - cardList[i].transform.position.x) * 6f));
                    targety = (float) rootPos.y - Mathf.Pow((float)(Mathf.Abs(cardList[i].transform.position.x - rootPos.x) * 0.2), 2f);
                }



                //setting x/y position
                cardList[i].transform.position = Vector3.Lerp(cardList[i].transform.position, new Vector3(targetx, targety, rootPos.z), Time.deltaTime * 30);
                cardList[i].transform.rotation = Quaternion.RotateTowards(cardList[i].transform.rotation, rotationQuaternion, Time.deltaTime * 200);  

            }
            else{
                cardList[i].transform.position = Vector3.Lerp(cardList[i].transform.position, new Vector3(start_x, rootPos.y, rootPos.z), Time.deltaTime * 30);
            }

            

















        }  
    }    






    /// <summary>  
    /// 销毁卡牌  
    /// </summary>  

    public  void RemoveCard()  
    {        
        if (cardList==null || cardList.Count == 0)  
        {
            return;  
        }  
        CardItem item = cardList[cardList.Count - 1];
        cardList.Remove(item); 
        StartCoroutine(item.Remove_Card());
    }    






	/// <summary>  
    /// 销毁卡牌  
    /// </summary>  
    /// <param name="item"></param>  

    public  void RemoveCard(CardItem item)  
    {        
        if (cardList==null || cardList.Count == 0)  
        {
	        return;  
        }   
        cardList.Remove(item);
        StartCoroutine(item.Remove_Card());
    }  








  
          






    /// <summary>  
    /// 玩家操作检测  
    /// </summary>  

    public void TaskItemDetection()  
    {       
       if (Input.GetKeyDown(KeyCode.A))  
        {         
           AddCard();  
        }    
        if (Input.GetKeyDown(KeyCode.D) && cardList != null && cardList.Count != 0)  
        {         
           RemoveCard(cardList[cardList.Count - 1]);
        }           
     }  
}
