using System;

namespace RetroEngine
{
    class InvalidSceneException : Exception
    {
        public InvalidSceneException()
        {
        }

        public InvalidSceneException(string message)
        : base(message)
        {
        }

        public InvalidSceneException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    class SceneParserException : Exception
    {
        public SceneParserException()
        {
        }

        public SceneParserException(string message)
        : base(message)
        {
        }

        public SceneParserException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
