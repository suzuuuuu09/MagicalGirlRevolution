using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro; //Include DamageNumbersPro Namespace     <-----     [REQUIRED]

namespace DamageNumbersPro.Demo
{
    public class EnemyDamage : MonoBehaviour
    {
        public DamageNumber popupPrefab; //Reference DamageNumber Prefab     <-----     [REQUIRED]

        //public Transform target;
        public EnemyStatus enemyStatus;


        public void SpawnPopup(float number)
        {
            DamageNumber newPopup = popupPrefab.Spawn(enemyStatus.transform.position, number);


            //You can do any change you want on the DamageNumber returned by the Spawn(...) function.
            //The following code is [OPTIONAL] just to show you some examples.


            //Let's make the popup follow the target.
            newPopup.SetFollowedTarget(enemyStatus.transform);

            //Let's check if the number is greater than 5.
            if (number > 5)
            {
                //Let's increase the popup's scale.
                newPopup.SetScale(1.3f);

                //Let's add some text to the right of the number.
                newPopup.enableRightText = true;
                newPopup.rightText = "!";

                //Let's change the color of the popup.
                newPopup.SetColor(new Color(1, 0.2f, 0.2f));
            }
            else
            {
                //The following lines reset the changes above.
                //This would only be neccesary for pooled popups.
                newPopup.SetScale(1);
                newPopup.enableRightText = false;
                newPopup.SetColor(new Color(1, 0.7f, 0.5f));
            }
        }
    }
}
