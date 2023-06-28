using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ChampionHitScanAttack : MonoBehaviour
{
    private ChampionAim championAim;
    [SerializeField] private Material weaponTracerMaterial;

    private void Awake() {
        championAim = GetComponent<ChampionAim>();
    }

    private void Start() {
        championAim.OnShoot += ChampionAim_OnShoot;
    }

    private void ChampionAim_OnShoot(object sender, ChampionAim.OnShootEventArgs e) {
        CreateWeaponTracer(e.weaponEndPointPosition, e.attackDir);

        Debug.Log("Weapon" + e.weaponEndPointPosition);
        Debug.Log("shoot" + e.attackDir);
        
    }

    private void CreateWeaponTracer(Vector3 fromPosition, Vector3 targetPosition) { 
        Vector3 shootDir = (targetPosition - fromPosition).normalized;

        float eulerZ = UtilsClass.GetAngleFromVectorFloat(shootDir) - 90;
        float weaponTracerSize = 5f;
        Vector3 tracerSpawnPosition = fromPosition + shootDir * weaponTracerSize * 0.5f;

       // weaponTracerMaterial.SetTextureScale()
        World_Mesh worldMesh = World_Mesh.Create(tracerSpawnPosition, eulerZ, 6f, weaponTracerSize, weaponTracerMaterial, null, 10000);

        float timer = .1f;
        FunctionUpdater.Create(() => {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                worldMesh.DestroySelf();
                return true;
            }
            return false;
        });
    }
}
