using UnityEngine;

[System.Serializable]
public class GameData {
    private int checkPoint;
    private int life;
    private int keyCount;
    private bool burning;
    private float xPosition;
    private float yPosition;
    private float zPosition;
    private float xRotation;
    private float yRotation;
    private float zRotation;
    private float wRotation;

    public GameData (GameObject Player, int cp) {
        PlayerState playerState = Player.GetComponent<PlayerState>();
        life = playerState.GetLife();
        keyCount = playerState.GetKeyCount();
        burning = playerState.burning;
        checkPoint = cp;
        xPosition = Player.transform.position.x;
        yPosition = Player.transform.position.y;
        zPosition = Player.transform.position.z;
        // xRotation = Player.transform.localRotation.x;
        // yRotation = Player.transform.localRotation.y;
        // zRotation = Player.transform.localRotation.z;
        // wRotation = Player.transform.localRotation.w;

    }

    public int GetLife () { return life; }
    public int GetKeyCount(){ return keyCount;}
    public int GetCheckPointNumber () { return checkPoint; }
    public bool GetBurning () { return burning; }
    public float GetXPosition () { return xPosition; }
    public float GetYPosition () { return yPosition; }
    public float GetZPosition () { return zPosition; }
    // public float GetXRotation () { return xRotation; }
    // public float GetYRotation () { return yRotation; }
    // public float GetZRotation () { return zRotation; }
    // public float GetWRotation () { return wRotation; }

        public void setLife(int l) { life = l; }

    public Vector3 getPosition(){
        return new Vector3(xPosition,yPosition,zPosition);
    }

    // public Quaternion getRotation(){
    //     return new Quaternion(xRotation,yRotation,zRotation,wRotation);
    // }
    // public void SetLife(int l) { life = l; }
}