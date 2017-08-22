using UnityEngine;

public class ShowHPMana : MonoBehaviour {
    [SerializeField]
    private Texture2D Hp;
    [SerializeField]
    private Texture2D ManaT;
    [SerializeField]
    private Texture2D painel;

    private Player player;
	public Player Player{
		set{player = value; }
	}
    private int maxHealth;
    private int maxMana;

    void OnGUI(){
        if (player != null) {
			int health = (int)player.CurrentHealth;
			maxHealth = (int)player.maxHealth;

			int mana = (int)player.CurrentMana;
			maxMana = (int)player.maxMana;

            //ResolucaoMestre.AutoResize(1024, 768);
            GUI.BeginGroup(new Rect(10, 10, 300, 109));

            GUI.DrawTexture(new Rect(0, 0, 300, 109), painel);
            GUI.DrawTexture(new Rect(97, 64, 188 * health / maxHealth, 13), Hp);
            GUI.DrawTexture(new Rect(87, 81, 188 * mana / maxMana, 13), ManaT);

            GUI.EndGroup();
        }
    }
}
