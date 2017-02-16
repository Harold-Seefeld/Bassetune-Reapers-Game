using UnityEngine;
using DungeonGeneration.Generator;
using DungeonGeneration.Renderer;
using DungeonGeneration.Logging;
using DungeonGeneration.Generator.Plotters;
using SocketIO;

namespace DungeonGeneration { 

    public class DungeonGeneratorBehaviour : MonoBehaviour {
        public int _mapHeight = 100;
        public int _mapWidth = 100;
        public int _roomsNumberMin = 5;
        public int _roomsNumberMax = 15;
        public int _roomSizeMin = 5;
        public int _roomSizeMax = 20;
        public int _corridorLengthMin = 2;
        public int _corridorLengthMax = 7;
        public int _seed = 123456;
        public bool _randomSeed = true;

        public GameObject _floorPrefab;
        public GameObject _wallPrefab;
        public GameObject _wallSeparatorPrefab;
        public GameObject _cornerInnPrefab;
        public GameObject _cornerOutPrefab;

        [SerializeField] private int[,] _tilesMap;
        private TilesMapGenerator _generator;
        private BRDungeonRenderer _renderer;
        private SocketIOComponent socket;

        void Awake() {
            _generator = new TilesMapGenerator();
            _renderer = BRDungeonRenderer.newInstance(this);
        }

        void Start() {
            socket = FindObjectOfType<SocketIOComponent>();
            socket.On("seed", SeedRecieved);
        }

        void SeedRecieved(SocketIOEvent ev)
        {
            _seed = (int)ev.data.GetField("s").n;
            generateDungeon();
        }

        /*
        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                generateDungeon();
            }
        }
        */

        private void generateDungeon() {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = Vector3.one;

            _generator.setMapSize(_mapHeight, _mapWidth);
            _generator.setRoomsNumberRange(_roomsNumberMin, _roomsNumberMax);
            _generator.setRoomSizeRange(_roomSizeMin, _roomSizeMax);
            _generator.setCorridorSizeRange(_corridorLengthMin, _corridorLengthMax);
            _generator.setPlotter(new DetailedTilesPlotter());
            //_generator.setLogger(new UnityEngineLogger());
            if (_randomSeed) {
                _seed = Time.time.ToString().GetHashCode();
            }
            _generator.setSeed(_seed);

            _tilesMap = _generator.asMatrix();
            _renderer.convertToMeshes(_tilesMap);

            transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.localScale = new Vector3(-1, 1, -1);
        }
    }
}