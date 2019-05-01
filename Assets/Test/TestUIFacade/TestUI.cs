using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BoBo.Light.UI;

public class TestUI : BaseView
{
    protected Button m_redButton;
    protected Button m_blueButton;


    protected override void OnOpened(object param, object extra)
    {
        m_redButton = this.transform.Find("RedButton").GetComponent<Button>();
        m_redButton.onClick.AddListener(() => { SendNotification(EventID.ChangeColor, Color.red); });

        m_blueButton = this.transform.Find("BlueButton").GetComponent<Button>();
        m_blueButton.onClick.AddListener(() => { SendNotification(EventID.ChangeColor, Color.blue); });

        PopPage("TestPage");
    }




    protected override void OnClosed()
    {
        m_redButton.onClick.RemoveAllListeners();
        m_redButton = null;
        m_blueButton.onClick.RemoveAllListeners();
        m_blueButton = null;
    }


    public override string GetUiID()
    {
        return "TestUI";
    }
}
