using UnityEngine;

class CameraFollower : Singleton<CameraFollower>
{
    private Player player;
    [SerializeField]
    [Range(0, 1)] private float smooth;
    [SerializeField]
    private Vector3 offset;
    public Player Player
    {
        set { player = value; }
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 p = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) + offset;
            transform.position = Vector3.Lerp(transform.position, p, smooth);
        }
    }
}