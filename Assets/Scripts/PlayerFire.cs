using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public enum WeaponType
    {
        Rifle,
        Bomb,
        Melee
    }

    public WeaponType currentWeapon = WeaponType.Rifle;

    public GameObject firePosition;

    public GameObject bombFactory;

    public float throwPower = 15f;

    public GameObject bulletEffect;
    ParticleSystem ps;

    public int weaponPower = 10;

    public float meleeRange = 2f;
    public int meleePower = 30;

    public GameObject knifeObject;
    Vector3 knifeOriginPos;

    public float swingDistance = 0.2f;
    public float swingTime = 0.05f;

    // 탄약
    public int rifleAmmo = 30;
    public int rifleMaxAmmo = 30;
    public int bombAmmo = 2;
    public int bombMaxAmmo = 2;

    public TMP_Text ammoText;


    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
        knifeObject.SetActive(false);
        knifeOriginPos = knifeObject.transform.localPosition;

        UpdateAmmoText();
    }

    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // 무기 전환
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = WeaponType.Rifle;
            print("무기 전환: 소총");
            knifeObject.SetActive(false);
            UpdateAmmoText();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = WeaponType.Bomb;
            print("무기 전환: 수류탄");
            knifeObject.SetActive(false);
            UpdateAmmoText();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon = WeaponType.Melee;
            print("무기 전환: 근접무기");
            knifeObject.SetActive(true);
            UpdateAmmoText();
        }

        // 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            switch (currentWeapon)
            {
                case WeaponType.Rifle:
                    rifleAmmo = rifleMaxAmmo;
                    print("재장전: 소총");
                    UpdateAmmoText();
                    break;
                case WeaponType.Bomb:
                    bombAmmo = bombMaxAmmo;
                    print("재장전: 수류탄");
                    UpdateAmmoText();
                    break;
            }
        }

        // 발사
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentWeapon)
            {
                case WeaponType.Rifle:
                    FireRifle();
                    break;
                case WeaponType.Bomb:
                    ThrowBomb();
                    break;
                case WeaponType.Melee:
                    MeleeAttack();
                    break;
            }
        }
    }

    void FireRifle()
    {
        if (rifleAmmo <= 0)
        {
            print("탄약 부족");
            return;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitinfo = new RaycastHit();

        if (Physics.Raycast(ray, out hitinfo))
        {
            if (hitinfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyFSM eFSM = hitinfo.transform.GetComponent<EnemyFSM>();
                eFSM.HitEnemy(weaponPower);
            }
            else
            {
                bulletEffect.transform.position = hitinfo.point;
                bulletEffect.transform.forward = hitinfo.normal;
                ps.Play();
            }
        }

        rifleAmmo--;
        UpdateAmmoText();
    }

    void ThrowBomb()
    {
        if (bombAmmo <= 0)
        {
            print("수류탄 부족");
            return;
        }

        GameObject bomb = Instantiate(bombFactory);
        bomb.transform.position = firePosition.transform.position;

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);

        // Player와 폭탄 충돌 무시
        Physics.IgnoreCollision(bomb.GetComponent<Collider>(), GetComponent<Collider>());

        bombAmmo--;
        UpdateAmmoText();
    }

    void MeleeAttack()
    {
        Vector3 attackPos = transform.position + transform.forward * meleeRange;
        Collider[] hits = Physics.OverlapSphere(attackPos, meleeRange);

        foreach (Collider c in hits)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyFSM eFSM = c.GetComponent<EnemyFSM>();
                eFSM.HitEnemy(meleePower);
            }
        }

        StopCoroutine("SwingKnife");
        StartCoroutine("SwingKnife");
    }

    IEnumerator SwingKnife()
    {
        Vector3 forwardPos = knifeOriginPos + new Vector3(0, 0, swingDistance);

        // 앞으로
        float t = 0;
        while (t < swingTime)
        {
            t += Time.deltaTime;
            knifeObject.transform.localPosition = Vector3.Lerp(knifeOriginPos, forwardPos, t / swingTime);
            yield return null;
        }

        // 뒤로
        t = 0;
        while (t < swingTime)
        {
            t += Time.deltaTime;
            knifeObject.transform.localPosition = Vector3.Lerp(forwardPos, knifeOriginPos, t / swingTime);
            yield return null;
        }

        knifeObject.transform.localPosition = knifeOriginPos;
    }

    void UpdateAmmoText()
    {
        switch (currentWeapon)
        {
            case WeaponType.Rifle:
                ammoText.text = rifleAmmo + "/" + rifleMaxAmmo;
                break;
            case WeaponType.Bomb:
                ammoText.text = bombAmmo + "/" + bombMaxAmmo;
                break;
            case WeaponType.Melee:
                ammoText.text = "∞";
                break;
        }
    }
}
