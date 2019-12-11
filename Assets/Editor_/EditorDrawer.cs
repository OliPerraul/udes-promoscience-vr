using UnityEngine;
using System.Collections;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths.Editor
{

    [System.Serializable]
    public class ObservableTileType : Cirrus.ObservableValue<TileType> 
    {
        public ObservableTileType(TileType value) : base(value) { }
    }

    public class EditorDrawer : Cirrus.BaseSingleton<EditorDrawer>
    {
        private TileType[] palette = {
                TileType.Start,

                TileType.End,

                TileType.Floor1,
                TileType.Floor2,
                TileType.Floor3,
                TileType.Floor4,
                TileType.Floor5,

                TileType.Horizontal1,
                TileType.Horizontal2,
                TileType.Horizontal3,
                TileType.Horizontal4,

                TileType.Vertical1,
                TileType.Vertical2,
                TileType.Vertical3,
                TileType.Vertical4,

                TileType.Corner1,
                TileType.Corner2,
                TileType.Corner3            
            };


        [SerializeField]
        private float heightOffset = 60f;

        [SerializeField]
        public ObservableTileType SelectedTileType = new ObservableTileType(TileType.Corner1);

        private int currentPaletteIndex = 0;

        private Cirrus.ObservableValue<Vector3> mousePosition = new Cirrus.ObservableValue<Vector3>();

        private Cirrus.ObservableValue<int> scroll = new Cirrus.ObservableValue<int>();

        private GameObject CursorTile;

        private Cirrus.ObservableValue<Vector2Int> tilePosition = new Cirrus.ObservableValue<Vector2Int>();

        private Cirrus.ObservableBool mouseHeld = new Cirrus.ObservableBool(false);

        public Cirrus.Event<Vector2Int, TileType> OnTileDrawnHandler;


        public virtual void Awake()
        {
            EditorController.Instance.State.OnValueChangedHandler += OnStateChanged;

            mousePosition.OnValueChangedHandler += OnMousePositionChanged;

            scroll.OnValueChangedHandler += OnScrollValueChanged;

            SelectedTileType.OnValueChangedHandler += OnCurrentTileChanged;

            tilePosition.OnValueChangedHandler += OnTilePositionChanged;

            mouseHeld.OnValueChangedHandler += OnMouseHeld;

        }

        public void Update()
        {
            mousePosition.Value = Input.mousePosition;

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                scroll.Value = +1;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                scroll.Value = -1;
            }
            else
            {
                scroll.Value = 0;
            }

            mouseHeld.Value = Input.GetMouseButton(0);
            if (Input.GetMouseButtonUp(0))
            {
                mouseHeld.Value = false;
            }
        }

        public virtual void OnStateChanged(EditorState state)
        {
            switch (state)
            {
                case EditorState.Select:
                    gameObject.SetActive(false);

                    break;

                case EditorState.Editor:
                    gameObject.SetActive(true);
                    break;

                default:
                    gameObject.SetActive(false);
                    break;
            }
        }

        public void OnMouseHeld(bool held)
        {
            if (held)
            {
                if (Input.GetMouseButton(0))
                {
                    Draw(tilePosition.Value, SelectedTileType.Value);
                }
            }
        }

        public void OnScrollValueChanged(int value)
        {
            if (value > 0)
            {
                currentPaletteIndex++;
                currentPaletteIndex = currentPaletteIndex.Mod(palette.Length);
                SelectedTileType.Value = palette[currentPaletteIndex];
            }
            else if (value < 0)
            {
                currentPaletteIndex--;
                currentPaletteIndex = currentPaletteIndex.Mod(palette.Length);
                SelectedTileType.Value = palette[currentPaletteIndex];
            }
        }
    
        public void OnMousePositionChanged(Vector3 pos)
        {
            RaycastHit hit;

            Ray ray = EditorController.Instance.LabyrinthObject.Camera.Source.ScreenPointToRay(pos);

            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, UnityEngine.Color.red);

            Debug.DrawRay(ray.origin, ray.direction * 100, UnityEngine.Color.red);


            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, Layers.LabyrinthFlags))
            {
                tilePosition.Value = EditorController.Instance.LabyrinthObject.GetWorldPositionInLabyrinthPosition(hit.point.x, hit.point.z);
            }
            else
            {
                if (CursorTile != null)
                {
                    CursorTile.gameObject.Destroy();
                    CursorTile = null;
                }
            }
        }

        public void OnTilePositionChanged(Vector2Int tilePosition)
        {
            if (mouseHeld.Value)
            {
                if (Input.GetMouseButton(0))
                {
                    Draw(tilePosition, SelectedTileType.Value);
                }
            }

            MoveDrawerTile(tilePosition.x, tilePosition.y);
        }

        public void OnCurrentTileChanged(TileType tile)
        {           
            Piece piece = EditorController.Instance.Labyrinth.Skin.GetPiece(tile);

            if (piece == null)
            {
                return;
            }           

            if (CursorTile != null)
            {
                CursorTile.gameObject.Destroy();
                CursorTile = null;
            }

            Vector3 wpos = EditorController.Instance.LabyrinthObject.GetLabyrinthPositionInWorldPosition(tilePosition.Value);

            CursorTile = Instantiate(
                piece.gameObject,
                wpos,
                Quaternion.identity,
                transform);


            var colliders = CursorTile.GetComponentsInChildren<Collider>();

            // Remove collider on cursor on collider tile
            foreach (var col in colliders)
            {
                if (col == null)
                    continue;

                col.enabled = false;
            }
        }

        private void MoveDrawerTile(int x, int y)
        {
            if (CursorTile == null)
            {
                OnCurrentTileChanged(SelectedTileType.Value);
            }

            if (CursorTile == null)
                return;

            Vector3 wpos = EditorController.Instance.LabyrinthObject.GetLabyrinthPositionInWorldPosition(x, y);

            CursorTile.transform.position = wpos;

            CursorTile.transform.position = CursorTile.transform.position + Vector3.up * heightOffset;
        }

        public void Draw(Vector2Int pos, TileType tile)
        {
            if (CursorTile == null)
                return;

            // TODO remove
            // Right now i'm just checking if the skin has the desired tile
            // Otherwise do not allow draw
            Piece piece = EditorController.Instance.Labyrinth.Skin.GetPiece(tile);

            if (piece == null)
            {
                return;
            }

            OnTileDrawnHandler?.Invoke(pos, tile);
        }

    }
}