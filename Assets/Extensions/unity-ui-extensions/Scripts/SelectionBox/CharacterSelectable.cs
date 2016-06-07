namespace UnityEngine.UI.Extensions
{
    public class CharacterSelectable : MonoBehaviour, IBoxSelectable
    {

        #region Implemented members of IBoxSelectable
        bool _selected = false;
        public bool selected
        {
            get
            {
                return _selected;
            }

            set
            {
                _selected = value;
            }
        }

        bool _preSelected = false;
        public bool preSelected
        {
            get
            {
                return _preSelected;
            }

            set
            {
                _preSelected = value;
            }
        }
        #endregion

        //We want the test object to be either a UI element, a 2D element or a 3D element, so we'll get the appropriate components
        SpriteRenderer spriteRenderer;
        Image image;
        Text text;
        CharacterData characterData;

        void Start()
        {
            spriteRenderer = transform.GetComponent<SpriteRenderer>();
            image = transform.GetComponent<Image>();
            text = transform.GetComponent<Text>();
            characterData = transform.GetComponent<CharacterData>();
        }

        void Update()
        {

            //What the game object does with the knowledge that it is selected is entirely up to it.
            //In this case we're just going to change the color.

            //White if deselected.
            Color color = Color.white;

            if (UseCaller.selectedCharacters.Contains(characterData))
            {
                UseCaller.selectedCharacters.Remove(characterData);
            }

            if (preSelected)
            {
                //Yellow if preselected
                color = Color.yellow;
            }
            if (selected)
            {
                //And green if selected.
                color = Color.green;

                if (!UseCaller.selectedCharacters.Contains(characterData))
                {
                    UseCaller.selectedCharacters.Add(characterData);
                }
            }

            //Set the color depending on what the game object has.
            if (spriteRenderer)
            {
                spriteRenderer.color = color;
            }
            else if (text)
            {
                text.color = color;
            }
            else if (image)
            {
                image.color = color;
            }
            else if (GetComponent<UnityEngine.Renderer>())
            {
                GetComponent<UnityEngine.Renderer>().material.color = color;
            }


        }
    }
}