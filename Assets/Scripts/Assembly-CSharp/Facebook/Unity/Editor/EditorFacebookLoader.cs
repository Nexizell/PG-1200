namespace Facebook.Unity.Editor
{
    public class EditorFacebookLoader : FB.CompiledFacebookLoader
    {
        protected override FacebookGameObject FBGameObject
        {
            get
            {
                EditorFacebookGameObject component = ComponentFactory.GetComponent<EditorFacebookGameObject>();
                component.Facebook = new EditorFacebook();
                return component;
            }
        }
    }
}
