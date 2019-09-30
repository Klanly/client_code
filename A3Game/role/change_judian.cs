using UnityEngine;
using System.Collections;

public class change_judian : MonoBehaviour
{

    public Material WIN_MTL_1, WIN_MTL_2, WIN_MTL_3, WIN_MTL_4, WIN_MTL_5, WIN_MTL_6, WIN_MTL_7;
    public Material LOSE_MTL_1, LOSE_MTL_2, LOSE_MTL_3, LOSE_MTL_4, LOSE_MTL_5, LOSE_MTL_6, LOSE_MTL_7;
    public Material NONE_1, NONE_4;
    void Start()
    {
        WIN_MTL_1 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_baoshi");
        LOSE_MTL_1 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_baoshi2");
        NONE_1 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_baoshi3");

        WIN_MTL_2 = U3DAPI.U3DResLoad<Material>("mtl/faguan_lan");
        LOSE_MTL_2 = U3DAPI.U3DResLoad<Material>("mtl/faguan_hong");

        WIN_MTL_3 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_fuwen");
        LOSE_MTL_3 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_fuwen2");

        WIN_MTL_4 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian");
        LOSE_MTL_4 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_hongdi");
        NONE_4 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_hongdi2");

        WIN_MTL_5 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian2");
        LOSE_MTL_5 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_hong");

        WIN_MTL_6 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_zhutou");
        LOSE_MTL_6 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_zhutou2");

        WIN_MTL_7 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_diquan");
        LOSE_MTL_7 = U3DAPI.U3DResLoad<Material>("mtl/zc_judian_diquan2");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void change(int tag, int type = 0)
    {
        Transform judian_obj = transform.FindChild("judian" + tag + "/zc_judian");

        if (judian_obj == null)
            return;
        if (type != 0)
        {
            judian_obj.GetComponent<Animator>().enabled = true;
            judian_obj.FindChild("baoshi/boashiguang").gameObject.SetActive(true);
            judian_obj.FindChild("baoshi_liuguang").gameObject.SetActive(true);
            judian_obj.FindChild("faguang_shuxian1").gameObject.SetActive(true);
            judian_obj.FindChild("faguang_shuxian2").gameObject.SetActive(true);
            judian_obj.FindChild("fuwen_faguang").gameObject.SetActive(true);
            judian_obj.FindChild("zc_judian_diquan").gameObject.SetActive(true);
            judian_obj.FindChild("zhutou/dian").gameObject.SetActive(true);
        }
        else
        {
            judian_obj.GetComponent<Animator>().enabled = false;
            judian_obj.FindChild("baoshi/boashiguang").gameObject.SetActive(false);
            judian_obj.FindChild("baoshi_liuguang").gameObject.SetActive(false);
            judian_obj.FindChild("faguang_shuxian1").gameObject.SetActive(false);
            judian_obj.FindChild("faguang_shuxian2").gameObject.SetActive(false);
            judian_obj.FindChild("fuwen_faguang").gameObject.SetActive(false);
            judian_obj.FindChild("zc_judian_diquan").gameObject.SetActive(false);
            judian_obj.FindChild("zhutou/dian").gameObject.SetActive(false);
        }


        if (type == 1)
        {
            Material mtl_inst_1 = GameObject.Instantiate(WIN_MTL_1) as Material;
            judian_obj.FindChild("baoshi").GetComponent<MeshRenderer>().material = mtl_inst_1;

            ParticleSystem thisParticle_1 = judian_obj.FindChild("baoshi/boashiguang").GetComponent<ParticleSystem>();
            thisParticle_1.startColor = new Vector4(0.67f, 0.99f, 1f, 0.22f);

            Material mtl_inst_2 = GameObject.Instantiate(WIN_MTL_2) as Material;
            judian_obj.FindChild("faguang_shuxian1").GetComponent<MeshRenderer>().material = mtl_inst_2;
            judian_obj.FindChild("faguang_shuxian2").GetComponent<MeshRenderer>().material = mtl_inst_2;

            ParticleSystem thisParticle_2 = judian_obj.FindChild("fuwen_faguang/fuwenguang").GetComponent<ParticleSystem>();
            thisParticle_2.startColor = new Vector4(0.67f, 0.99f, 1f, 1f);

            Material mtl_inst_3 = GameObject.Instantiate(WIN_MTL_3) as Material;
            thisParticle_2.GetComponent<Renderer>().material = mtl_inst_3;

            Material mtl_inst_4 = GameObject.Instantiate(WIN_MTL_4) as Material;
            judian_obj.FindChild("judian1").GetComponent<MeshRenderer>().material = mtl_inst_4;

            Material mtl_inst_5 = GameObject.Instantiate(WIN_MTL_5) as Material;
            judian_obj.FindChild("judian2").GetComponent<MeshRenderer>().material = mtl_inst_5;

            Material mtl_inst_6 = GameObject.Instantiate(WIN_MTL_6) as Material;
            judian_obj.FindChild("zhutou").GetComponent<MeshRenderer>().material = mtl_inst_6;

            ParticleSystem thisParticle_3 = judian_obj.FindChild("zhutou/dian").GetComponent<ParticleSystem>();
            thisParticle_3.startColor = new Vector4(0, 0.75f, 1f, 1f);

            ParticleSystem thisParticle_4 = judian_obj.FindChild("zc_judian_diquan").GetComponent<ParticleSystem>();
            thisParticle_4.startColor = new Vector4(0, 0.62f, 0.62f, 1f);

            Material mtl_inst_7 = GameObject.Instantiate(WIN_MTL_7) as Material;
            thisParticle_4.GetComponent<Renderer>().material = mtl_inst_7;
        }
        else if (type == 2)
        {
            Material mtl_inst_1 = GameObject.Instantiate(LOSE_MTL_1) as Material;
            judian_obj.FindChild("baoshi").GetComponent<MeshRenderer>().material = mtl_inst_1;

            ParticleSystem thisParticle = judian_obj.FindChild("baoshi/boashiguang").GetComponent<ParticleSystem>();
            thisParticle.startColor = new Vector4(1f, 0.9f, 0.46f, 0.65f);

            Material mtl_inst_2 = GameObject.Instantiate(LOSE_MTL_2) as Material;
            judian_obj.FindChild("faguang_shuxian1").GetComponent<MeshRenderer>().material = mtl_inst_2;
            judian_obj.FindChild("faguang_shuxian2").GetComponent<MeshRenderer>().material = mtl_inst_2;

            ParticleSystem thisParticle_2 = judian_obj.FindChild("fuwen_faguang/fuwenguang").GetComponent<ParticleSystem>();
            thisParticle_2.startColor = new Vector4(1, 1, 1, 1);

            Material mtl_inst_3 = GameObject.Instantiate(LOSE_MTL_3) as Material;
            thisParticle_2.GetComponent<Renderer>().material = mtl_inst_3;

            Material mtl_inst_4 = GameObject.Instantiate(LOSE_MTL_4) as Material;
            judian_obj.FindChild("judian1").GetComponent<MeshRenderer>().material = mtl_inst_4;

            Material mtl_inst_5 = GameObject.Instantiate(LOSE_MTL_5) as Material;
            judian_obj.FindChild("judian2").GetComponent<MeshRenderer>().material = mtl_inst_5;

            Material mtl_inst_6 = GameObject.Instantiate(LOSE_MTL_6) as Material;
            judian_obj.FindChild("zhutou").GetComponent<MeshRenderer>().material = mtl_inst_6;

            ParticleSystem thisParticle_3 = judian_obj.FindChild("zhutou/dian").GetComponent<ParticleSystem>();
            thisParticle_3.startColor = new Vector4(1, 0.72f, 0.3f, 1f);

            ParticleSystem thisParticle_4 = judian_obj.FindChild("zc_judian_diquan").GetComponent<ParticleSystem>();
            thisParticle_4.startColor = new Vector4(1, 0.55f, 0.32f, 1f);

            Material mtl_inst_7 = GameObject.Instantiate(LOSE_MTL_7) as Material;
            thisParticle_4.GetComponent<Renderer>().material = mtl_inst_7;
        }
        else
        {
            Material mtl_inst_1 = GameObject.Instantiate(NONE_1) as Material;
            judian_obj.FindChild("baoshi").GetComponent<MeshRenderer>().material = mtl_inst_1;

            Material mtl_inst_4 = GameObject.Instantiate(NONE_4) as Material;
            judian_obj.FindChild("judian1").GetComponent<MeshRenderer>().material = mtl_inst_4;

            Material mtl_inst_5 = GameObject.Instantiate(NONE_1) as Material;
            judian_obj.FindChild("judian2").GetComponent<MeshRenderer>().material = mtl_inst_5;
        }
    }
}
