using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public InputField longueur;
    public InputField largeur;
    public InputField erreur;

    public Toggle id1;
    public Toggle id2;
    public Toggle id3;
    public Toggle id4;

    public Toggle longerWay;
    public Toggle byOrder;
    public Toggle smallTravel;
    public Toggle random;

    public Toggle brit;
    public Toggle kart;
    public Toggle ptol;
    public Toggle rome;

    public Toggle cycle;

    void Start(){
    	longueur.text = "10";
    	largeur.text = "10";
    	erreur.text = "5";
    }

    public void generate(){	
    	int lo = int.Parse(longueur.text);
    	int la = int.Parse(largeur.text);
    	int er = int.Parse(erreur.text);
    	int resol = getResolution();
    	int id = getId();
    	int st = getStyle();
    	int cy = 0;
    	if(cycle.isOn) cy = 1;
    	LabyrinthTranslator lab = new LabyrinthTranslator(lo,la,resol,er,st,cy);
    	SQLiteUtilities.InsertOrReplaceLabyrinth(id, lo * 2 + 1, la * 2 + 1, lab.translate());
    	Debug.Log("Insert done");
    }

    public bool checkInputFiel(){
    	if(longueur.text == "" || largeur.text == "" || erreur.text == "") return true;
    	for(int i = 0; i < longueur.text.Length; i++){
    		if(longueur.text[i]< '0' || longueur.text[i] > '9') return true;
    	}
    	for(int i = 0; i < largeur.text.Length; i++){
    		if(largeur.text[i] < '0' || largeur.text[i] > '9') return true;
    	}
    	for(int i = 0; i < erreur.text.Length; i++){
    		if(erreur.text[i] < '0' || erreur.text[i] > '9') return true;
    	}
    	return false;
    }

    public void correctLongeur(){
    	if(longueur.text != ""){
    		for(int i = 0; i< longueur.text.Length;){
    			if(longueur.text[i] < '0' || longueur.text[i] > '9') longueur.text = longueur.text.Remove(i,1);
    			else i++;
    		}
		}
		if(longueur.text == "") longueur.text = "10";
		if(int.Parse(longueur.text)>100) longueur.text = "100";
		correctErreur();
    }

    public void correctLargeur(){
    	if(largeur.text != ""){
    		for(int i = 0; i< largeur.text.Length;){
    			if(largeur.text[i] < '0' || largeur.text[i] > '9') largeur.text = largeur.text.Remove(i,1);
    			else i++;
    		}
		}
		if(largeur.text == "") largeur.text = "10";
		if(int.Parse(largeur.text)>100) largeur.text = "100";
		correctErreur();
    }

    public void correctErreur(){
    	if(largeur.text != "" && longueur.text != "" && erreur.text != ""){
    		if(int.Parse(erreur.text) > 0.1 * int.Parse(largeur.text) * int.Parse(longueur.text)){
                int res = (int) 0.1 * int.Parse(largeur.text) * int.Parse(longueur.text);
                if (res == 0) res = 1;
                erreur.text = "" + res;
    		}
    	}
    	if(erreur.text == "") erreur.text = "5";
    }

    public void setTutoParams(){
    	if(id4.isOn){
    		longerWay.enabled = false;
    		byOrder.enabled = false;
    		smallTravel.enabled = false;
    		random.enabled = false;
    	}
    	else{
    		longerWay.enabled = true;
    		byOrder.enabled = true;
    		smallTravel.enabled = true;
    		random.enabled = true;
    	}
    }

    public int getResolution(){
    	if(id4.isOn) return 0;
    	if(longerWay.isOn) return 1;
    	if(byOrder.isOn) return 2;
    	if(random.isOn) return 4;
    	return 3;
    }

    public int getId(){
    	if(id1.isOn) return 1;
    	if(id2.isOn) return 2;
    	if(id3.isOn) return 3;
    	return 4;
    }

    public int getStyle(){
    	if(brit.isOn) return 3;
    	if(kart.isOn) return 4;
    	if(ptol.isOn) return 2;
    	return 1;
    }
}
