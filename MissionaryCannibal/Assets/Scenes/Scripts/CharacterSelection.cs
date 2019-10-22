using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{

    public List<GameObject> MissionaryLeft = new List<GameObject>(3);
    public List<GameObject> MissionaryRight = new List<GameObject>(3);
    public List<GameObject> CannibalLeft = new List<GameObject>(3);
    public List<GameObject> CannibalRight = new List<GameObject>(3);
    public GameObject boat;
    readonly Vector2 boatTop = new Vector2(-0.1f,.25f);
    readonly Vector2 leftBank = new Vector2(-7.5f,-2.8f);
    readonly Vector2 rightBank = new Vector2(7.5f,-2.8f);
    public bool bankSideRight = false;
    public bool AllowSelection = true;
    public List<GameObject> peopleOnBoard = new List<GameObject>(2);

    void Start()
    {
        MissionaryLeft.Add(GameObject.Find("M1"));
        MissionaryLeft.Add(GameObject.Find("M2"));
        MissionaryLeft.Add(GameObject.Find("M3"));
        CannibalLeft.Add(GameObject.Find("C1"));
        CannibalLeft.Add(GameObject.Find("C2"));
        CannibalLeft.Add(GameObject.Find("C3"));
        
    }
    // Update is called once per frame
    void Update()
    {
            if (Input.GetMouseButtonDown(0)&&AllowSelection )
            {
            
                RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
                // Debug.Log(rayHit.transform.name);
                // Debug.Log(rayHit.transform.position);
                if (rayHit)
                {
                    GameObject selectedObject = rayHit.transform.gameObject;
                    if (rayHit.collider.CompareTag("Boat"))
                    {
                        //boat goes here
                        if (peopleOnBoard.Count > 0)
                        {
                            if (!bankSideRight)
                            {
                                //Moving boat to Right Bank
                                //rayHit.transform.position = Vector2.Lerp(rayHit.transform.position, new Vector2(2.0f, -3.5f), 1f);
                                StartCoroutine(MoveBoat(selectedObject,new Vector2(2.0f, -3.5f)));
                                bankSideRight = true;
                            }
                            else
                            {
                                //Moving boat to Left bank
                                //rayHit.transform.position = Vector2.Lerp(rayHit.transform.position, new Vector2(-2.0f, -3.5f), 1f);
                                StartCoroutine(MoveBoat(selectedObject,new Vector2(-2.0f, -3.5f)));
                                bankSideRight = false;
                            }
                        }

                    }
                    else if(rayHit.collider.CompareTag("Missionary"))
                    {
                        //Missionary
                        if ( peopleOnBoard.Count < 2 && !peopleOnBoard.Contains(selectedObject) && !bankSideRight && MissionaryLeft.Contains(selectedObject))
                        {
                            //Adding Missionary to the boat from the Left bank
                            rayHit.transform.parent = boat.transform;
                            rayHit.transform.localPosition = boatTop;
                            MissionaryLeft.Remove(selectedObject);
                            // MissionaryRight.Add(selectedObject);
                            peopleOnBoard.Add(selectedObject);
                        }
                        else if ( peopleOnBoard.Count < 2 && !peopleOnBoard.Contains(selectedObject) && bankSideRight && MissionaryRight.Contains(selectedObject))
                        {
                            //Adding Missionary to the boat from the Right Bank
                            rayHit.transform.parent = boat.transform;
                            rayHit.transform.localPosition = boatTop;
                            MissionaryRight.Remove(selectedObject);
                            // MissionaryLeft.Add(selectedObject);
                            peopleOnBoard.Add(selectedObject);
                        }
                        else if (peopleOnBoard.Contains(selectedObject) && !bankSideRight)
                        {
                            //Adding Missionary to the Left Bank from the boat
                            rayHit.transform.position = leftBank;
                            rayHit.transform.parent = null;
                            peopleOnBoard.Remove(selectedObject);
                            MissionaryLeft.Add(selectedObject);
                        }
                        else if (peopleOnBoard.Contains(selectedObject) && bankSideRight)
                        {
                            //Adding Missionary to the Right bank from the boat
                            rayHit.transform.position = rightBank;
                            rayHit.transform.parent = null;
                            peopleOnBoard.Remove(selectedObject);
                            MissionaryRight.Add(selectedObject);
                        }
                    }
                    else if (rayHit.collider.CompareTag("Cannibal"))
                    {
                        //cannibal
                        if (peopleOnBoard.Count < 2 && !peopleOnBoard.Contains(selectedObject) && !bankSideRight && CannibalLeft.Contains(selectedObject))
                        {
                            //Adding Cannibal to the boat from Left Bank
                            rayHit.transform.parent = boat.transform;
                            rayHit.transform.localPosition = boatTop;
                            CannibalLeft.Remove(selectedObject);
                            // CannibalRight.Add(selectedObject);
                            peopleOnBoard.Add(selectedObject);
                        }
                        else if (peopleOnBoard.Count < 2 && !peopleOnBoard.Contains(selectedObject) && bankSideRight && CannibalRight.Contains(selectedObject))
                        {
                            // Adding Cannibal to boat from the right bank
                            rayHit.transform.parent = boat.transform;
                            rayHit.transform.localPosition = boatTop;
                            CannibalRight.Remove(selectedObject);
                            // CannibalLeft.Add(selectedObject);
                            peopleOnBoard.Add(selectedObject);
                        }
                        else if (peopleOnBoard.Contains(selectedObject) && !bankSideRight)
                        {
                            //Adding Cannibal to the Left bank from boat
                            rayHit.transform.position = leftBank;
                            rayHit.transform.parent = null;
                            peopleOnBoard.Remove(selectedObject);
                            CannibalLeft.Add(selectedObject);
                        }
                        else if (peopleOnBoard.Contains(selectedObject) && bankSideRight)
                        {
                            // Adding Cannibal to the Right bank from boat
                            rayHit.transform.position = rightBank;
                            rayHit.transform.parent = null;
                            peopleOnBoard.Remove(selectedObject);
                            CannibalRight.Add(selectedObject);
                        }
                    }

                }
            }
    }

    IEnumerator MoveBoat(GameObject boat,Vector2 PositionToMove)
    {

        if((MissionaryLeft.Count == 1 && CannibalLeft.Count == 2)||(MissionaryRight.Count == 1 && CannibalRight.Count == 2) || (MissionaryLeft.Count == 1 && CannibalLeft.Count == 3)||(MissionaryRight.Count == 1 && CannibalRight.Count == 3) || (MissionaryLeft.Count == 2 && CannibalLeft.Count == 3)||(MissionaryRight.Count == 2 && CannibalRight.Count == 3)){
            Debug.Log("Game Over!!!");
        }
        else{
        AllowSelection = false;
        
        while(true)
        {
            boat.transform.position = Vector2.MoveTowards(boat.transform.position,PositionToMove,Time.deltaTime);
            if(Vector2.Distance(boat.transform.position,PositionToMove)<0.05f)
            {   
                boat.transform.position = PositionToMove;
                break;
            }
            yield return null;

        }
        //TeleportCharacterOnBoard();
        AllowSelection = true;
        }        
         
    }
}

// public void TeleportCharacterOnBoard(){
//     if(bankSideRight){
//         if(peopleOnBoard.Count == 1){
        
//         }
//     }
// }
