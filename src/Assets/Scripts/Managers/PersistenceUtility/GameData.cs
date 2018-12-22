using UnityEngine;

[System.Serializable]
public class GameData {
    private int checkPointNumber;
    private int life;
    private int keyCount;
    private int bombCount;
    private bool burning;
    private float xPosition;
    private float yPosition;
    private float zPosition;
    // private float xRotation;
    // private float yRotation;
    // private float zRotation;
    // private float wRotation;

    public GameData (GameObject Player, int cp) {
        life = Managers.PlayerManager.GetLife ();
        keyCount = Managers.PlayerManager.GetKeyCount ();
        bombCount = Managers.PlayerManager.GetBombCount();
        burning = Managers.PlayerManager.burning;
        checkPointNumber = cp;
        xPosition = Player.transform.position.x;
        yPosition = Player.transform.position.y;
        zPosition = Player.transform.position.z;
        // xRotation = Player.transform.localRotation.x;
        // yRotation = Player.transform.localRotation.y;
        // zRotation = Player.transform.localRotation.z;
        // wRotation = Player.transform.localRotation.w;

    }

    public int GetLife () { return life; }
    public int GetCheckPointNumber () { return checkPointNumber; }
    public int GetKeyCount () { return keyCount; }
    public bool GetBurning () { return burning; }
    public int GetBombCount () { return bombCount; }
    public float GetXPosition () { return xPosition; }
    public float GetYPosition () { return yPosition; }
    public float GetZPosition () { return zPosition; }
    // public float GetXRotation () { return xRotation; }
    // public float GetYRotation () { return yRotation; }
    // public float GetZRotation () { return zRotation; }
    // public float GetWRotation () { return wRotation; }

    public void setLife (int l) { life = l; }

    public Vector3 getPosition () {
        return new Vector3 (xPosition, yPosition, zPosition);
    }

    public void Dump () {
        string str = "";
        str += "Game Data at checkpoint:" + GetCheckPointNumber ();
        str += "\nLife:" + GetLife ();
        str += "\nKeyCount:" + GetKeyCount ();
        str += "\nBombCount:" + GetBombCount ();
        str += "\nburning?: " + GetBurning ();
        str += "\nPosition:" + getPosition ();
        Debug.Log (str+"\n\n");
    }

    // public Quaternion getRotation(){
    //     return new Quaternion(xRotation,yRotation,zRotation,wRotation);
    // }
    // public void SetLife(int l) { life = l; }
}