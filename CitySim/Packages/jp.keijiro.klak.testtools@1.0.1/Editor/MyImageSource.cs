using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

namespace Klak.TestTools
{

    public sealed class MyImageSource : MonoBehaviour
    {
        #region Public property

        public Texture Texture => OutputBuffer;

        public Camera _vrCam;
        #endregion

        #region Editable attributes



        // Camera options
        [SerializeField] Camera _camera = null;

        // Output options
        [SerializeField] RenderTexture _outputTexture = null;
        [SerializeField] Vector2Int _outputResolution = new Vector2Int(1920, 1080);

        #endregion

        #region Package asset reference

        [SerializeField, HideInInspector] Shader _shader = null;

        #endregion

        #region Private members

        UnityWebRequest _webTexture;
        WebCamTexture _webcam;
        Material _material;
        RenderTexture _buffer;

        RenderTexture OutputBuffer
          => _outputTexture != null ? _outputTexture : _buffer;

        // Blit a texture into the output buffer with aspect ratio compensation.
        void Blit(Texture source, bool vflip = false)
        {
            if (source == null) return;

            var aspect1 = (float)source.width / source.height;
            var aspect2 = (float)OutputBuffer.width / OutputBuffer.height;
            var gap = aspect2 / aspect1;

            var scale = new Vector2(gap, vflip ? -1 : 1);
            var offset = new Vector2((1 - gap) / 2, vflip ? 1 : 0);

            Graphics.Blit(source, OutputBuffer, scale, offset);
        }

        #endregion

        #region MonoBehaviour implementation

        void Start()
        {
            // Allocate a render texture if no output texture has been given.
            if (_outputTexture == null)
                _buffer = new RenderTexture
                  (_outputResolution.x, _outputResolution.y, 0);




        }

        void OnDestroy()
        {
            if (_webcam != null) Destroy(_webcam);
            if (_buffer != null) Destroy(_buffer);
            if (_material != null) Destroy(_material);
        }

        void Update()
        {

            // Asynchronous image downloading
            if (_webTexture != null && _webTexture.isDone)
            {
                var texture = DownloadHandlerTexture.GetContent(_webTexture);
                _webTexture.Dispose();
                _webTexture = null;
                Blit(texture);
                Destroy(texture);
            }

        }

        #endregion
    }

} // namespace Klak.TestTools
