using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Health health = null;

        [System.Serializable]
        private struct CursorMap
        {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField]
        private CursorMap[] cursorMapping = new CursorMap[0];
        [SerializeField]
        private float navMeshProjectionDistance = 1f;


        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Awake()
        {
            this.health = this.GetComponent<Health>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (this.InteractWithUI())
            {
                this.SetCursor(CursorType.UI);
                return;
            }

            if (this.health.IsDead())
            {
                this.SetCursor(CursorType.None);
                return;
            }

            if (this.InteractWithComponent()) return;
            if (this.InteractWithMovement()) return;

            this.SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastSorted();
            foreach (var hit in hits)
            {
                var raycastables = hit.collider.GetComponents<IRaycastable>();
                if (raycastables == null) continue;
                if (raycastables.Length <= 0) continue;

                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        this.SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit raycastHit;
            if (!Physics.Raycast(this.GetMouseRay(), out raycastHit)) return false;

            NavMeshHit navMeshHit;
            bool navMeshHitFlag = 
                NavMesh.SamplePosition(raycastHit.point, out navMeshHit, this.navMeshProjectionDistance, NavMesh.AllAreas);
            if (!navMeshHitFlag)
                return false;

            target = navMeshHit.position;

            return true;
        }

        private RaycastHit[] RaycastSorted()
        {
            var hits = Physics.RaycastAll(this.GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            // distances配列とhits配列は要素のインデックスは対応している
            // distances配列を値が小さい順にして小さい順にした配列の各要素のインデックスに従って
            // hits配列をソートする
            // つまりdistances配列の値からソートするindexを抽出してhitsのソートをする
            // 使用条件はソートの基準配列とソート対象の配列の要素数が同じで関連性がある事
            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool InteractWithMovement()
        {
            Vector3 target = Vector3.zero;
            if (this.RaycastNavMesh(out target))
            {
                if (!this.GetComponent<Mover>().CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    this.GetComponent<Mover>().StartMoveAction(target, 1f);
                }

                this.SetCursor(CursorType.Movment);

                return true;
            }

            return false;
        }
        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            var cursorMap = this.GetCursorMap(type);
            Cursor.SetCursor(cursorMap.texture, cursorMap.hotspot, CursorMode.Auto);
        }

        private CursorMap GetCursorMap(CursorType type)
        {
            foreach (var map in this.cursorMapping)
            {
                if (map.cursorType == type) return map;
            }

            return this.cursorMapping[0];
        }
    }
}
