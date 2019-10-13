using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*처음 젠가 세팅
 * 
 * 
 * 
 */
public class Builder : MonoBehaviour
{
    public GameObject JengaLayerPrefab;
    public GameObject LastJengaLayer;//최상층 레이어는 처음엔 인스펙터에서 지정되고 이후에 코드에서 바뀜 
    public int CountFalledStick;

    // Start is called before the first frame update
    void Start()
    {
        CountFalledStick = 0;

        //시작하자마자 한층 배치
        MakeNewLayer();
        LastJengaLayer.GetComponent<SelectStickToActive>().PlaceStick();
        LastJengaLayer.GetComponent<SelectStickToActive>().PlaceStick();
        LastJengaLayer.GetComponent<SelectStickToActive>().PlaceStick();
    }

    //스틱을 위에 쌓기
    public void BuildOnTop()
    {
        Debug.Log("BuildOnTop");
        LastJengaLayer.GetComponent<SelectStickToActive>().PlaceStick();
    }

    //새로운 층 생성
    public void MakeNewLayer()
    {
        //생성될 층의 위치와 각도
        Quaternion TopLayerRotation = Quaternion.Euler(
            LastJengaLayer.transform.eulerAngles.x,
            LastJengaLayer.transform.eulerAngles.y + 90,
            LastJengaLayer.transform.eulerAngles.z
        );
        Vector3 TopLayerPosition = new Vector3(
            LastJengaLayer.transform.position.x,
            LastJengaLayer.transform.position.y + 0.5f,
            LastJengaLayer.transform.position.z
        );

        //새로운 층 생성을 생성하여 태그를 toplayer로 변경
        GameObject NewLayer =(GameObject) Instantiate(JengaLayerPrefab, TopLayerPosition, TopLayerRotation);

        //새로 배치된 층과 밑 층의 태그를 새로 설정하고 각 층의 스틱들 태그 조정
        NewLayer.tag = "TopLayer";
        NewLayer.GetComponent<SelectStickToActive>().DeactiveStickToRaycast();
        LastJengaLayer.tag = "Layer";
        LastJengaLayer.GetComponent<SelectStickToActive>().ActiveStickToRaycast();

        //만들어진 층을 마지막 레이어로 저장
        LastJengaLayer = NewLayer;
    }
    
}
