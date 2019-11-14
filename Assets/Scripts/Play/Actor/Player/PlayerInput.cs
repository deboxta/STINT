using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    //Author : Anthony Bérubé
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private KeyCode changeTimelineKeyboardKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode freezeTimeKeyboardKey = KeyCode.Q;
        [SerializeField] private float inputThreshold = 0.13f;
        
        private GamePadState gamePadState;
        private PlayerMover playerMover;
        private Player player;
        private bool viewingRight;
        private bool crouching;
        //BC : Utilise "Clicked" ou "Pressed", mais pas les deux dans tes noms de variables.
        //     J'ai une préférence pour "Pressed".
        private bool timeChangeIsClicked;
        private bool freezeTimeIsClicked;
        private bool jumpButtonIsPressed;
        
        private void Awake()
        {
            playerMover = GetComponent<PlayerMover>();
            player = GetComponent<Player>();

            viewingRight = false;
            crouching = false;
            timeChangeIsClicked = false;
            freezeTimeIsClicked = false;
        }

        //BC : La méthode "Update" mériterait d'être découpée en de plus petites méthodes.
        private void Update()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);

            //Crouch
            //BR : Vous êtes vraiment chanceux que "Xinput" gère lui même ses "Deadzones". Sinon, la comparaison entre X et 0
            //     ne marcherais jamais.
            if (gamePadState.ThumbSticks.Left.Y < 0 && gamePadState.ThumbSticks.Left.X == 0)
                crouching = true;
            else
                crouching = false;

            var direction = Vector2.zero;
            //Right
            //BC : Beaucoup de "SerializedFields" manquants ici (pour les KeyCode). Vous en avez quelques un en haut pourtant. 
            if (Input.GetKey(KeyCode.D) ||
                gamePadState.ThumbSticks.Left.X > inputThreshold)
            {
                direction += Vector2.right;
                player.IsLookingRight = true;
                //BC : Il y a un "IsLookingRight" ici. Cela me semble similaire à "FlipPlayer".
                if (transform.localScale.x < 0)
                    player.FlipPlayer();
            }

            //Left
            if (Input.GetKey(KeyCode.A) ||
                gamePadState.ThumbSticks.Left.X < -inputThreshold)
            {
                direction += Vector2.left;
                player.IsLookingRight = false;
                //BC : Il y a un "IsLookingRight" ici. Cela me semble similaire à "FlipPlayer".
                if (transform.localScale.x > 0)
                    player.FlipPlayer();
            }
            playerMover.Move(direction);

            //Jump
            if ((Input.GetKeyDown(KeyCode.Space) || gamePadState.Buttons.A == ButtonState.Pressed) && !jumpButtonIsPressed)
            {
                playerMover.Jump();
                jumpButtonIsPressed = true;
            }

            if (gamePadState.Buttons.A == ButtonState.Released)
                jumpButtonIsPressed = false;
            
            //Switch timeline
            if (gamePadState.Buttons.X == ButtonState.Pressed ||
                gamePadState.Buttons.Y == ButtonState.Pressed)
                timeChangeIsClicked = true;
            else if (Input.GetKeyDown(changeTimelineKeyboardKey) ||
                     gamePadState.Buttons.X == ButtonState.Released && timeChangeIsClicked ||
                     gamePadState.Buttons.Y == ButtonState.Released && timeChangeIsClicked)
            {
                Finder.TimelineController.SwitchTimeline();
                timeChangeIsClicked = false;
            }
            
            // TODO temp
            //Freeze time
            if (gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
                freezeTimeIsClicked = true;
            else if (Input.GetKeyDown(freezeTimeKeyboardKey) ||
                     gamePadState.Buttons.LeftShoulder == ButtonState.Released && freezeTimeIsClicked)
            {
                Finder.TimeFreezeController.SwitchState();
                freezeTimeIsClicked = false;
            }

            //Fall
            if (gamePadState.Buttons.A == ButtonState.Released)
                playerMover.Fall();

            //Grab
            if (Input.GetKeyDown(KeyCode.C) ||
                GamePad.GetState(PlayerIndex.One).Triggers.Right > 0)
                player.GrabBox();

            //Throw
            if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0 == false && player.Hands.IsHoldingBox)
                //BC : Voir mes commentaires dans cette méthode.
                player.ThrowBox(crouching);
        }
    }
}