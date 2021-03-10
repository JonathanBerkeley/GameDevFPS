using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to make code neater and more readable
namespace FlagTranslations
{
    internal enum ServerCodeTranslations
    {
        serverFull = 1,
        invalidUsername,
        usernameTaken,
        badRequest,
        wait,
        retry,
        errorDenied,
        generalDenied
    }

    internal enum ClientCodeTranslations
    {
        noError,
        connectionRefused,
        badForms
    }
}
