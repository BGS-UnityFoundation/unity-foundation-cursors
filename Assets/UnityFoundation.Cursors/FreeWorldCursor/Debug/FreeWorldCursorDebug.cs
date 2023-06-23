using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace UnityFoundation.Cursors
{
    public class FreeWorldCursorDebug : Singleton<FreeWorldCursorDebug>
    {
        private IWorldCursor worldCursor;
        private GameObject debugVisual;

        [SerializeField] private GameObject worldCursorObj;
        [field: SerializeField] private bool DebugMode { get; set; }

        protected override void OnAwake()
        {
            GetWorldCursorRef();

            worldCursor.OnClick += HandleClick;
            worldCursor.OnSecondaryClick += HandleSecondaryClick;

            debugVisual = transform.Find("debug_visual").gameObject;
            debugVisual.SetActive(DebugMode);
        }

        private void GetWorldCursorRef()
        {
            worldCursor = worldCursorObj != null
                ? worldCursorObj.GetComponent<IWorldCursor>()
                : GameObjectUtils.FindInScene<IWorldCursor>();

            if(worldCursor == null)
                throw new ArgumentNullException("World cursor is not set in the scene to debug.");
        }

        public void Update()
        {
            if(!DebugMode) return;
            if(worldCursor == null)
            {
                GetWorldCursorRef();
                return;
            }

            worldCursor.WorldPosition.Some(pos => {
                debugVisual.SetActive(true);
                debugVisual.transform.position = pos;
            })
            .OrElse(() => debugVisual.SetActive(false));
        }

        private void HandleClick() => UnityDebug.I.LogHighlight("[FreeWorldCursor]", "clicked");

        private void HandleSecondaryClick() => UnityDebug.I.LogHighlight("[FreeWorldCursor]", "secondary clicked");
    }
}