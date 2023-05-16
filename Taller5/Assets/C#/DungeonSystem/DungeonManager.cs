using Code_Boses;
using Code_Core;
using Code_UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code_DungeonSystem
{
    public class DungeonManager : MonoBehaviour
    {
        public static DungeonManager instance { get; private set; } = null;
        public GameObject Gicamu { get => _gicamu;}
        public GameObject Alchies { get => _alchies; }

        [Header("Dungeon Configuration")]
        [Space(10)]
        [SerializeField] private int numbersOfRooms;
        [SerializeField] private float initialSecretRoomProbability;
        [SerializeField] private float incrisingProbabilityRate;
        [SerializeField] private float currentProbability;
        [Header("Rooms Collection")]
        [Space(10)]
        [SerializeField] private GameObject spawnRoom;
        [SerializeField] private List<GameObject> enemyRooms = new List<GameObject>();
        [SerializeField] private GameObject bossRoom;
        [SerializeField] private GameObject secretRoom;
        [SerializeField] private List<Room> dungeonRooms;
        [Header("Characters")]
        [SerializeField] private GameObject gicamuPrefab;
        [SerializeField] private GameObject alchiesPrefab;
        private GameObject _gicamu;
        private GameObject _alchies;
        [SerializeField] private Vector3 spawnOffset;
        [Header("Game System")]
        [SerializeField] private float secondOfTransition;
        private Transform cam;
        private float _currentTime;
        private int level = 0;
        [Header("Bosses")]
        [SerializeField] GameObject Boss01Prefab;
        [SerializeField] GameObject Boss02Prefab;
        [Header("Reliquias")]
        [SerializeField] List<GameObject> Relics;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }

            instance = this;
        }

        void Start()
        {
            cam = Camera.main.transform;
            currentProbability = initialSecretRoomProbability;
            _currentTime= Time.timeScale;
            GenerateDungeon();
        }

        public void onChangingRoom(int i) 
        {
            StartCoroutine(ChangingRoom(i));
        }

        IEnumerator ChangingRoom(int i)
        {
            TransitionManager.instance.FadeIn();
            yield return new WaitForSeconds(secondOfTransition);
            /*cam.position = dungeonRooms[i].secretRoom? new Vector3(cam.position.x, cam.position.y -10, -10) : new Vector3(cam.position.x + 20, cam.position.y,-10);
            if (cam.position.y < 0)
            {
                cam.position = new Vector3(cam.position.x, cam.position.y + 10, -10);
            }*/

            Vector3 center = dungeonRooms[i+1].roomBounds.center;
            _gicamu.transform.position = center + spawnOffset + dungeonRooms[i + 1].transform.position;
            _alchies.transform.position = center - spawnOffset + dungeonRooms[i + 1].transform.position;
            cam.gameObject.GetComponent<Camera>().orthographicSize = dungeonRooms[i + 1].CameraScale;
            cam.position = new Vector3(center.x + dungeonRooms[i + 1].transform.position.x, dungeonRooms[i + 1].CamY, -10);
            PlayerController _gicamuPlayerController = _gicamu.GetComponent<PlayerController>();
            PlayerController _alchiesPlayerController = _alchies.GetComponent<PlayerController>();
            dungeonRooms[i+1].ActivateEnemies(_gicamuPlayerController, _alchiesPlayerController);
            TransitionManager.instance.FadeOut();
            yield return new WaitForSeconds(secondOfTransition);
        }

        IEnumerator SpawnPlayers(Room initRoom) 
        {
            Vector3 center = initRoom.roomBounds.center;
            _gicamu = Instantiate(gicamuPrefab, center + spawnOffset, Quaternion.identity);
            _gicamu.transform.parent = null;
            _alchies = Instantiate(alchiesPrefab, center - spawnOffset, Quaternion.identity);
            _alchies.transform.parent = null;
            yield return new WaitForSeconds(2);
            _gicamu.GetComponent<Wizard>().pauseControllers = false;
            _alchies.GetComponent<Alchemist>().pauseControllers = false;
        }

        private void GenerateDungeon() 
        {
            int count = 0;

            Vector3 spawnRoomPosition= Vector3.zero;
            GameObject r;
            r = Instantiate(spawnRoom,spawnRoomPosition,Quaternion.identity);
            r.transform.parent = null;
            Room room = r.GetComponent<Room>();
            dungeonRooms.Add(room);
            StartCoroutine(SpawnPlayers(room));
            room.ID = count;
            DataBase.Instance.ChangeID(count);
            spawnRoomPosition = new Vector3(spawnRoomPosition.x + r.transform.localScale.x + 25, spawnRoomPosition.y);
            Shuffle(enemyRooms);

            count++;

            for (int i = 0; i < enemyRooms.Count; i++)
            {

                r = Instantiate(enemyRooms[i], spawnRoomPosition, Quaternion.identity);
                r.transform.parent = null;
                room = r.GetComponent<Room>();
                room.ID = count;
                dungeonRooms.Add(room);
                spawnRoomPosition = new Vector3(spawnRoomPosition.x + r.transform.localScale.x + 25, spawnRoomPosition.y);
                int x = Random.Range(0, 101);
                if (x<= currentProbability)
                {
                    count++;
                    Vector3 spawnSecretRoomPosition = new Vector3(r.transform.position.x,-r.transform.localScale.y-5);
                    r = Instantiate(secretRoom, spawnSecretRoomPosition, Quaternion.identity);
                    r.transform.parent = null;
                    room = r.GetComponent<Room>();
                    room.ID = count;
                    int ran = Random.Range(0,Relics.Count);
                    room.relic = Relics[ran];
                    dungeonRooms.Add(room);
                    dungeonRooms[count - 1].secretRoom = true;
                    currentProbability = initialSecretRoomProbability;
                }
                else
                {
                    currentProbability += incrisingProbabilityRate;
                }

                count++;
            }

            r = Instantiate(bossRoom, spawnRoomPosition, Quaternion.identity);
            r.transform.parent = null;
            room = r.GetComponent<Room>();
            room.ID = count;
            room.boss= true;
            room.bossOBJ = level == 0 ? Instantiate(Boss01Prefab, room.pointA.transform.position, Quaternion.identity).GetComponent<BossStateManager>()
                : Instantiate(Boss02Prefab, room.pointA.transform.position, Quaternion.identity).GetComponent<BossStateManager>();
            dungeonRooms.Add(room);

        }

        public void Shuffle(List<GameObject> list) 
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject temp = list[i];
                int rand = Random.Range(i, list.Count);
                list[i] = list[rand];
                list[rand] = temp;
            }
        }
    }
}
