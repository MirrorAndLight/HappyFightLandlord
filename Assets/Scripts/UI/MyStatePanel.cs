using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyStatePanel : StatePanel
{
    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SHOW_GRAB_BUTTON);
        Bind(UIEvent.SHOW_DEAL_BUTTON);
       // Bind(UIEvent.SET_MY_PLAYER_DATA);
        Bind(UIEvent.PLAYER_HIDE_READY_BUTTON);
    }
    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);

        switch (eventCode)
        {
            //case UIEvent.SET_MY_PLAYER_DATA:
            //    this.userDto = message as UserDto;
            //    break;
            case UIEvent.SHOW_GRAB_BUTTON:
                {
                    bool active = (bool)message;
                    btnGrab.gameObject.SetActive(active);
                    btnNGrab.gameObject.SetActive(active);
                }
                break;
            case UIEvent.SHOW_DEAL_BUTTON:
                {
                    bool active = (bool)message;
                    btnDeal.gameObject.SetActive(active);
                    btnNDeal.gameObject.SetActive(active);
                }
                break;
            case UIEvent.PLAYER_HIDE_READY_BUTTON:
                btnReady.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    private Button btnDeal;
    private Button btnNDeal;
    private Button btnGrab;
    private Button btnNGrab;
    private Button btnReady;

    private SocketMsg socketMsg;

    protected override void Start()
    {
        base.Start();

        btnDeal = transform.Find("btnDeal").GetComponent<Button>();
        btnNDeal = transform.Find("btnNDeal").GetComponent<Button>();
        btnGrab = transform.Find("btnGrab").GetComponent<Button>();
        btnNGrab = transform.Find("btnNGrab").GetComponent<Button>();
        btnReady = transform.Find("btnReady").GetComponent<Button>();

        btnDeal.onClick.AddListener(DealClick);
        btnNDeal.onClick.AddListener(NDealClick);

        btnGrab.onClick.AddListener(() =>{
            GrabClick(true);
        });
        btnNGrab.onClick.AddListener(() =>
        {
            GrabClick(false);
        });

        btnReady.onClick.AddListener(ReadyClick);

        socketMsg = new SocketMsg();

        //????????????
        btnGrab.gameObject.SetActive(false);
        btnNGrab.gameObject.SetActive(false);
        btnDeal.gameObject.SetActive(false);
        btnNDeal.gameObject.SetActive(false);

        //?????????????????????
        UserDto myUserDto = Models.GameModel.MatchRoomDto.UIdUserDict[Models.GameModel.UserDto.Id];
        this.userDto = myUserDto;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnDeal.onClick.RemoveAllListeners();
        btnNDeal.onClick.RemoveAllListeners();
        btnGrab.onClick.RemoveAllListeners();
        btnNGrab.onClick.RemoveAllListeners();
        btnReady.onClick.RemoveAllListeners();
    }

    protected override void ReadyState()
    {
        base.ReadyState();
        btnReady.gameObject.SetActive(false);
    }

    private void DealClick()
    {
        //??????????????????????????????
        Dispatch(AreaCode.CHARACTER, CharacterEvent.DEAL_CARD, null);

        btnDeal.gameObject.SetActive(false);
        btnNDeal.gameObject.SetActive(false);
    }
    private void NDealClick()
    {
        //?????????????????????????????????
        socketMsg.Change(OpCode.FIGHT, FightCode.PASS_CREQ, null);
        Dispatch(AreaCode.NET, 0, socketMsg);

        btnDeal.gameObject.SetActive(false);
        btnNDeal.gameObject.SetActive(false);
    }
    private void GrabClick(bool result)
    {
        if(result==true)
        {
            //?????????
            socketMsg.Change(OpCode.FIGHT, FightCode.GRAB_LANDLORD_CREQ, true);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }
        else
        {
            //??????
            socketMsg.Change(OpCode.FIGHT, FightCode.GRAB_LANDLORD_CREQ, false);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }

        btnGrab.gameObject.SetActive(false);
        btnNGrab.gameObject.SetActive(false);
    }
    private void ReadyClick()
    {
        //??????????????????????????????
        socketMsg.Change(OpCode.MATCH, MatchCode.READY_CREQ, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
}
