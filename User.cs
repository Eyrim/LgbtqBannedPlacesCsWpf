using System;

namespace LgbtqBannedPlacesCsWpf
{
    public class User
    {
        public bool IsCis;
        public bool IsHet;
        public bool PresentsCis;

        /// <summary>
        /// Implementation of user
        /// </summary>
        /// <param name="cis">bool; true if the user is cisgender</param>
        /// <param name="het">bool; true if the user is heterosexual AND heteromantic</param>
        /// <param name="pCis">bool; true if the user isn't GNC</param>
        public User(bool cis, bool het, bool pCis)
        {
            IsCis = cis;
            IsHet = het;
            PresentsCis = pCis;
        }
    }
}
