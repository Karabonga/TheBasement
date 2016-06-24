using System.Collections.Generic;
using System.Windows.Forms;

namespace RetroEngine
{
    class Input
    {
        Dictionary<string, Keys> keyNames;
        Dictionary<Keys, bool> keyDown;
        Dictionary<Keys, bool> lastFrameDown;

        public Input()
        {
            keyNames = new Dictionary<string, Keys>();
            keyDown = new Dictionary<Keys, bool>();
            lastFrameDown = new Dictionary<Keys, bool>();
        }

        /// <summary>
        /// Adds a new input key to be observed.
        /// </summary>
        /// <param name="name">The name of the new input.</param>
        /// <param name="key">The key to be observed.</param>
        public void RegisterInput(string name, Keys key)
        {
            keyNames.Add(name, key);
            keyDown.Add(key, false);
            lastFrameDown.Add(key, false);
        }

        /// <summary>
        /// Sets the input key to the specified value.
        /// </summary>
        /// <param name="key">The key to be set to a value.</param>
        /// <param name="value">The value of the key.</param>
        public void SetKey(Keys key, bool value)
        {
            if (keyDown.ContainsKey(key))
                keyDown[key] = value;
        }

        /// <summary>
        /// Indicates whether the key is down or not.
        /// </summary>
        /// <param name="name">The name of the key.</param>
        /// <returns>Returns a boolean indicating whether the key is down or not.</returns>
        public bool GetKeyDown(string name)
        {
            if (keyNames.ContainsKey(name))
                return keyDown[keyNames[name]];
            else
                return false;
        }

        /// <summary>
        /// Indicates whether the key has been pressed or not.
        /// </summary>
        /// <param name="name">The name of the key.</param>
        /// <returns>Return a boolean indicating whether the key has been pressed or not.</returns>
        public bool GetKeyPressed(string name)
        {
            if (keyNames.ContainsKey(name))
                return lastFrameDown[keyNames[name]] && !keyDown[keyNames[name]];
            else
                return false;
        }

        /// <summary>
        /// Gets the name of the specified key.
        /// </summary>
        /// <param name="key">The key with the name to be searched for.</param>
        /// <returns>Returns the name of a specified key ('' if there is no such key).</returns>
        public string GetKeyName(Keys key)
        {
            if (keyNames.ContainsValue(key))
            {
                foreach (string s in keyNames.Keys)
                {
                    if (keyNames[s] == key)
                        return s;
                }
            }
            return "";
        }

        /// <summary>
        /// A final update refreshing the input before ending the current frame.
        /// </summary>
        public void FinalUpdate()
        {
            //Sets the last frame down if keys have been pressed
            foreach (Keys k in keyDown.Keys)
            {
                lastFrameDown[k] = keyDown[k];
            }
        }
    }
}
