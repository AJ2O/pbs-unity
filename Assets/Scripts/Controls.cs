// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Battle"",
            ""id"": ""d91b38ca-65f6-4774-ae9d-2b234daa043a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""68328de9-6558-4937-880d-312d7e84c460"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""eabcdd18-b86d-4c20-85fb-903324e29100"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Special"",
                    ""type"": ""Button"",
                    ""id"": ""e3f227a1-2bf5-4c33-8132-c3a73c26931b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""1d36cdd0-f78b-4dcf-8f0f-11c17cf3f94b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""9611b323-752a-4bea-a5fe-0994a8602f43"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9d49c44c-dcd0-4a85-94b0-5fb0b2a0f78d"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""32a92b59-b389-4996-82f8-eed02a6086fc"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""80039475-1834-4e9e-974a-22ee2ee22525"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fb5c33a5-9aa2-4cec-afa6-3b8b3189a238"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2bb25c57-2024-43f2-8f3c-ff4b8ced9c5b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6184c280-4425-4ede-9845-895b512edc3b"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""adf6693d-73af-4433-8fe5-493138522c43"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Special"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""7fce0f1b-f97f-4833-8831-39730f7d6ac5"",
            ""actions"": [
                {
                    ""name"": ""DisplayBattle"",
                    ""type"": ""Button"",
                    ""id"": ""1c72f4fc-1a91-41eb-b635-33633ae20d9a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f0fcc950-af47-4454-b9cb-be2118576de6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""DisplayBattle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BattleDialog"",
            ""id"": ""1b094b57-1280-4a9c-9b1c-903283b0b8a6"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""5f62e070-93e4-4a4e-8813-c8c9c0505b73"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""70d95fea-30a2-473f-8567-3153e6b242eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6c27220e-14ac-418b-bba1-fa9413d4034b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0ecd439-1c49-4943-bfe4-616778b762f7"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BattleMenuCommand"",
            ""id"": ""e8f45869-8141-470d-bd97-a70409d5e7aa"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""54986389-9ba0-4752-9e1d-b645ffb4d10e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""b023bd16-6782-4cfd-a9d9-da000b4b99ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""e0565683-d762-4657-ac08-be520cd0a1ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0104c68f-6755-434e-9951-7522badc5eff"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d865b5e9-6e8a-4541-8c94-88fbb1bbeb84"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""988d8d64-2fdd-4144-a0d5-2ca94b8e0159"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a56505e3-a782-44da-a138-0903975299c5"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3fd2058f-bfbe-4a34-b9c2-b56217b0ad8c"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e218db0c-9642-4593-bdf8-eacf3ea4e67b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""51f925df-a1c4-449f-8c4b-4d3988fbf2b4"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""BattleMenuFight"",
            ""id"": ""fca0bd3f-bd21-4cd1-b0d7-1cae9cce4f24"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""1e11cee6-7ea9-4541-8f46-4fb0c2904eff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""af2dd51b-27e2-4d43-ae11-1b7940d4f624"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Special"",
                    ""type"": ""Button"",
                    ""id"": ""89e9408b-69da-41f8-9057-152a4a59262f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""1aa10fb5-37cb-4fa0-aa1b-27be1cf15152"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""69df6c76-e917-4a0c-a9ab-d718238e1f55"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f22882fe-9f24-4131-ac44-a421952ca8c5"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""beb9ca61-0671-4adc-bd9f-9a906ff0afce"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2cf33d28-c58b-4b02-ace8-fa20b54b2bc0"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""68682ba9-3aed-4c25-a475-d7ab0fae4bbb"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ef940af7-afed-4ef6-ad1a-d3fd3c75ae13"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb92b6a7-5d51-4c24-aea4-3ec00a006224"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1136ed01-98f8-441a-a4cc-d24c61e4484f"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Special"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BattleMenuFieldTarget"",
            ""id"": ""c4523e7a-e3eb-4db3-bad7-526e82e278aa"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""e5f63d44-b596-4add-ad76-7fcffed4d497"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""83eafb95-afcb-4a92-b031-6e125a949d03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""14775acf-6d01-4bd5-9949-7cce7cb0b9ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""5aebb928-0165-4939-84ae-b20db662be2a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cdddbc2f-9b0f-4e76-ba84-31f073214137"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4bba77f4-d493-4dc5-81ff-9df1fd8657df"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0b05b0d1-4891-4353-a5e3-f44cae6ac045"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fbaef204-201a-46f3-bd19-fb1b66e8bd03"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""24fbbc01-3f91-4b53-8c38-117d024e613c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6f2f900-2af3-4472-a7ec-62367b3b4578"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BattleMenuParty"",
            ""id"": ""4bf251bb-94bc-4443-8c54-94e400532247"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""9b5f1786-39d9-4281-873b-3a4e0bd5cdc6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""0421db3f-982f-40d6-b0c3-603c09d4e071"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""1fc42f91-409d-4885-a20e-6696351cba09"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""f41b8ff9-0113-4af4-ae51-043dccc3fe75"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f42b83fd-8dba-47fd-9c80-53e6fbef67a2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5567ff3f-4bdd-42d8-b86c-67889eb7c4ba"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3615d120-aa99-4351-aa5d-7db89915a8c8"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""36f3fccb-392a-4327-b312-7629c14bb948"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""08379cf8-aeb3-4f6d-aabc-42cf94bc229c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66cc9fdf-ab3f-4d86-bbb0-59a4d7d59566"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BattleMenuPartyCommand"",
            ""id"": ""95949b0d-a582-499a-a4d5-faef1d6e86f1"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""68b73b4b-3100-4cc3-a0f2-f03e0b1b6c99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""2a80efe1-df59-4567-a325-8515d6e3c827"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""c38c1a9e-4988-414a-aab1-c0f9bb000576"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e5ae78c8-0016-4ffb-be88-123b49dc77fb"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58860608-0aaf-413c-b611-56c69bfcdb43"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""076770cf-7a39-4bbc-b4be-1e7bb280d859"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d7ea74fd-720a-41a1-9910-0f575df05eb4"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""311deb19-74c9-4aef-ad77-1058f02b7083"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4f89d175-2af6-457e-9911-c2c26a9ef7aa"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7f922f31-271e-4e5d-aaca-2bb2b7ed266f"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""BattleMenuBag"",
            ""id"": ""601de63d-a5a5-4f35-9aac-7ab10ad04a10"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""168a12da-551b-44ac-a4e6-e870c22f1eba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""fa9545ce-e854-4605-9113-650f124b7070"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""b5f81f3f-ab1e-454d-8702-0e47713ae188"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""64a79187-46f2-4394-8e2b-5fbb7cc79b5c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""09ce90b0-f2f7-4907-be6a-20bd01e8e5ea"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""04269f8a-ab59-4b18-9c4d-091396d80c10"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f03b81ce-fae1-47a5-a464-1d6bc976b239"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""87fd30f5-579b-48fe-82dc-e60fabcac762"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""eaf35439-11f1-47c2-a2cb-6fb1169850f4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ed3f66a-e526-4d5c-b52b-d156e52d9a87"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BattleMenuBagItem"",
            ""id"": ""99d74d18-80ae-4ee3-95e1-f823f1575b47"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""49aef255-e917-4e20-8ce7-eca8a2717cef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""5a74def0-d144-44f7-b0aa-f40bce20433f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""c5fdbd80-a11d-4b0b-b90f-514275bb3111"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""856f4f06-41ff-4450-b7dc-da1f7fcf91dd"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c7936c21-18d0-4098-ab91-a06f0735d0f3"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""30527721-e588-4a18-8421-32b389e8619b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1033e385-6164-4129-af72-ecc65c47b5f9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2c953d86-6e7e-4ff7-935a-6e952034981b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e796c1db-4343-407a-947a-6db5d755e2e9"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4647ee2-4e35-46e6-8cc9-604cd4ba839f"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BattleMenuBagCommand"",
            ""id"": ""ac1e852b-b3aa-4602-9991-87d5254cc7b0"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""8bf93d8b-c463-47a6-8915-262d997b5c56"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""732b635d-e13c-42e6-97bb-e87e81a561b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""0dad35b7-8722-4642-bb1c-65e84229723a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""d9e1e73e-6766-494d-8333-8378a78a0fe9"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""243f6f81-d178-4ae8-9c26-92fc056fdbbc"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""31fc2d4e-bdc5-4c4b-b196-97f4cb83f6b2"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f6421f20-fedd-4879-b10f-0fdb05260cd6"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3315d4ae-5699-4aa8-ac8f-57900bb1bfe4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Default"",
            ""bindingGroup"": ""Default"",
            ""devices"": []
        }
    ]
}");
        // Battle
        m_Battle = asset.FindActionMap("Battle", throwIfNotFound: true);
        m_Battle_Move = m_Battle.FindAction("Move", throwIfNotFound: true);
        m_Battle_Select = m_Battle.FindAction("Select", throwIfNotFound: true);
        m_Battle_Special = m_Battle.FindAction("Special", throwIfNotFound: true);
        m_Battle_Cancel = m_Battle.FindAction("Cancel", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_DisplayBattle = m_Debug.FindAction("DisplayBattle", throwIfNotFound: true);
        // BattleDialog
        m_BattleDialog = asset.FindActionMap("BattleDialog", throwIfNotFound: true);
        m_BattleDialog_Select = m_BattleDialog.FindAction("Select", throwIfNotFound: true);
        m_BattleDialog_Cancel = m_BattleDialog.FindAction("Cancel", throwIfNotFound: true);
        // BattleMenuCommand
        m_BattleMenuCommand = asset.FindActionMap("BattleMenuCommand", throwIfNotFound: true);
        m_BattleMenuCommand_Move = m_BattleMenuCommand.FindAction("Move", throwIfNotFound: true);
        m_BattleMenuCommand_Select = m_BattleMenuCommand.FindAction("Select", throwIfNotFound: true);
        m_BattleMenuCommand_Cancel = m_BattleMenuCommand.FindAction("Cancel", throwIfNotFound: true);
        // BattleMenuFight
        m_BattleMenuFight = asset.FindActionMap("BattleMenuFight", throwIfNotFound: true);
        m_BattleMenuFight_Move = m_BattleMenuFight.FindAction("Move", throwIfNotFound: true);
        m_BattleMenuFight_Select = m_BattleMenuFight.FindAction("Select", throwIfNotFound: true);
        m_BattleMenuFight_Special = m_BattleMenuFight.FindAction("Special", throwIfNotFound: true);
        m_BattleMenuFight_Cancel = m_BattleMenuFight.FindAction("Cancel", throwIfNotFound: true);
        // BattleMenuFieldTarget
        m_BattleMenuFieldTarget = asset.FindActionMap("BattleMenuFieldTarget", throwIfNotFound: true);
        m_BattleMenuFieldTarget_Move = m_BattleMenuFieldTarget.FindAction("Move", throwIfNotFound: true);
        m_BattleMenuFieldTarget_Select = m_BattleMenuFieldTarget.FindAction("Select", throwIfNotFound: true);
        m_BattleMenuFieldTarget_Cancel = m_BattleMenuFieldTarget.FindAction("Cancel", throwIfNotFound: true);
        // BattleMenuParty
        m_BattleMenuParty = asset.FindActionMap("BattleMenuParty", throwIfNotFound: true);
        m_BattleMenuParty_Move = m_BattleMenuParty.FindAction("Move", throwIfNotFound: true);
        m_BattleMenuParty_Select = m_BattleMenuParty.FindAction("Select", throwIfNotFound: true);
        m_BattleMenuParty_Cancel = m_BattleMenuParty.FindAction("Cancel", throwIfNotFound: true);
        // BattleMenuPartyCommand
        m_BattleMenuPartyCommand = asset.FindActionMap("BattleMenuPartyCommand", throwIfNotFound: true);
        m_BattleMenuPartyCommand_Move = m_BattleMenuPartyCommand.FindAction("Move", throwIfNotFound: true);
        m_BattleMenuPartyCommand_Select = m_BattleMenuPartyCommand.FindAction("Select", throwIfNotFound: true);
        m_BattleMenuPartyCommand_Cancel = m_BattleMenuPartyCommand.FindAction("Cancel", throwIfNotFound: true);
        // BattleMenuBag
        m_BattleMenuBag = asset.FindActionMap("BattleMenuBag", throwIfNotFound: true);
        m_BattleMenuBag_Move = m_BattleMenuBag.FindAction("Move", throwIfNotFound: true);
        m_BattleMenuBag_Select = m_BattleMenuBag.FindAction("Select", throwIfNotFound: true);
        m_BattleMenuBag_Cancel = m_BattleMenuBag.FindAction("Cancel", throwIfNotFound: true);
        // BattleMenuBagItem
        m_BattleMenuBagItem = asset.FindActionMap("BattleMenuBagItem", throwIfNotFound: true);
        m_BattleMenuBagItem_Move = m_BattleMenuBagItem.FindAction("Move", throwIfNotFound: true);
        m_BattleMenuBagItem_Select = m_BattleMenuBagItem.FindAction("Select", throwIfNotFound: true);
        m_BattleMenuBagItem_Cancel = m_BattleMenuBagItem.FindAction("Cancel", throwIfNotFound: true);
        // BattleMenuBagCommand
        m_BattleMenuBagCommand = asset.FindActionMap("BattleMenuBagCommand", throwIfNotFound: true);
        m_BattleMenuBagCommand_Move = m_BattleMenuBagCommand.FindAction("Move", throwIfNotFound: true);
        m_BattleMenuBagCommand_Select = m_BattleMenuBagCommand.FindAction("Select", throwIfNotFound: true);
        m_BattleMenuBagCommand_Cancel = m_BattleMenuBagCommand.FindAction("Cancel", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Battle
    private readonly InputActionMap m_Battle;
    private IBattleActions m_BattleActionsCallbackInterface;
    private readonly InputAction m_Battle_Move;
    private readonly InputAction m_Battle_Select;
    private readonly InputAction m_Battle_Special;
    private readonly InputAction m_Battle_Cancel;
    public struct BattleActions
    {
        private @Controls m_Wrapper;
        public BattleActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Battle_Move;
        public InputAction @Select => m_Wrapper.m_Battle_Select;
        public InputAction @Special => m_Wrapper.m_Battle_Special;
        public InputAction @Cancel => m_Wrapper.m_Battle_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_Battle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleActions set) { return set.Get(); }
        public void SetCallbacks(IBattleActions instance)
        {
            if (m_Wrapper.m_BattleActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnMove;
                @Select.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnSelect;
                @Special.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnSpecial;
                @Special.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnSpecial;
                @Special.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnSpecial;
                @Cancel.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_BattleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Special.started += instance.OnSpecial;
                @Special.performed += instance.OnSpecial;
                @Special.canceled += instance.OnSpecial;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public BattleActions @Battle => new BattleActions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_DisplayBattle;
    public struct DebugActions
    {
        private @Controls m_Wrapper;
        public DebugActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @DisplayBattle => m_Wrapper.m_Debug_DisplayBattle;
        public InputActionMap Get() { return m_Wrapper.m_Debug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
        public void SetCallbacks(IDebugActions instance)
        {
            if (m_Wrapper.m_DebugActionsCallbackInterface != null)
            {
                @DisplayBattle.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnDisplayBattle;
                @DisplayBattle.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnDisplayBattle;
                @DisplayBattle.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnDisplayBattle;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @DisplayBattle.started += instance.OnDisplayBattle;
                @DisplayBattle.performed += instance.OnDisplayBattle;
                @DisplayBattle.canceled += instance.OnDisplayBattle;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);

    // BattleDialog
    private readonly InputActionMap m_BattleDialog;
    private IBattleDialogActions m_BattleDialogActionsCallbackInterface;
    private readonly InputAction m_BattleDialog_Select;
    private readonly InputAction m_BattleDialog_Cancel;
    public struct BattleDialogActions
    {
        private @Controls m_Wrapper;
        public BattleDialogActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_BattleDialog_Select;
        public InputAction @Cancel => m_Wrapper.m_BattleDialog_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_BattleDialog; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleDialogActions set) { return set.Get(); }
        public void SetCallbacks(IBattleDialogActions instance)
        {
            if (m_Wrapper.m_BattleDialogActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_BattleDialogActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleDialogActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleDialogActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_BattleDialogActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleDialogActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleDialogActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_BattleDialogActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public BattleDialogActions @BattleDialog => new BattleDialogActions(this);

    // BattleMenuCommand
    private readonly InputActionMap m_BattleMenuCommand;
    private IBattleMenuCommandActions m_BattleMenuCommandActionsCallbackInterface;
    private readonly InputAction m_BattleMenuCommand_Move;
    private readonly InputAction m_BattleMenuCommand_Select;
    private readonly InputAction m_BattleMenuCommand_Cancel;
    public struct BattleMenuCommandActions
    {
        private @Controls m_Wrapper;
        public BattleMenuCommandActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BattleMenuCommand_Move;
        public InputAction @Select => m_Wrapper.m_BattleMenuCommand_Select;
        public InputAction @Cancel => m_Wrapper.m_BattleMenuCommand_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_BattleMenuCommand; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleMenuCommandActions set) { return set.Get(); }
        public void SetCallbacks(IBattleMenuCommandActions instance)
        {
            if (m_Wrapper.m_BattleMenuCommandActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BattleMenuCommandActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BattleMenuCommandActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BattleMenuCommandActionsCallbackInterface.OnMove;
                @Select.started -= m_Wrapper.m_BattleMenuCommandActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleMenuCommandActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleMenuCommandActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_BattleMenuCommandActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleMenuCommandActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleMenuCommandActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_BattleMenuCommandActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public BattleMenuCommandActions @BattleMenuCommand => new BattleMenuCommandActions(this);

    // BattleMenuFight
    private readonly InputActionMap m_BattleMenuFight;
    private IBattleMenuFightActions m_BattleMenuFightActionsCallbackInterface;
    private readonly InputAction m_BattleMenuFight_Move;
    private readonly InputAction m_BattleMenuFight_Select;
    private readonly InputAction m_BattleMenuFight_Special;
    private readonly InputAction m_BattleMenuFight_Cancel;
    public struct BattleMenuFightActions
    {
        private @Controls m_Wrapper;
        public BattleMenuFightActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BattleMenuFight_Move;
        public InputAction @Select => m_Wrapper.m_BattleMenuFight_Select;
        public InputAction @Special => m_Wrapper.m_BattleMenuFight_Special;
        public InputAction @Cancel => m_Wrapper.m_BattleMenuFight_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_BattleMenuFight; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleMenuFightActions set) { return set.Get(); }
        public void SetCallbacks(IBattleMenuFightActions instance)
        {
            if (m_Wrapper.m_BattleMenuFightActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnMove;
                @Select.started -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnSelect;
                @Special.started -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnSpecial;
                @Special.performed -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnSpecial;
                @Special.canceled -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnSpecial;
                @Cancel.started -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleMenuFightActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_BattleMenuFightActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Special.started += instance.OnSpecial;
                @Special.performed += instance.OnSpecial;
                @Special.canceled += instance.OnSpecial;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public BattleMenuFightActions @BattleMenuFight => new BattleMenuFightActions(this);

    // BattleMenuFieldTarget
    private readonly InputActionMap m_BattleMenuFieldTarget;
    private IBattleMenuFieldTargetActions m_BattleMenuFieldTargetActionsCallbackInterface;
    private readonly InputAction m_BattleMenuFieldTarget_Move;
    private readonly InputAction m_BattleMenuFieldTarget_Select;
    private readonly InputAction m_BattleMenuFieldTarget_Cancel;
    public struct BattleMenuFieldTargetActions
    {
        private @Controls m_Wrapper;
        public BattleMenuFieldTargetActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BattleMenuFieldTarget_Move;
        public InputAction @Select => m_Wrapper.m_BattleMenuFieldTarget_Select;
        public InputAction @Cancel => m_Wrapper.m_BattleMenuFieldTarget_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_BattleMenuFieldTarget; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleMenuFieldTargetActions set) { return set.Get(); }
        public void SetCallbacks(IBattleMenuFieldTargetActions instance)
        {
            if (m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface.OnMove;
                @Select.started -= m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_BattleMenuFieldTargetActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public BattleMenuFieldTargetActions @BattleMenuFieldTarget => new BattleMenuFieldTargetActions(this);

    // BattleMenuParty
    private readonly InputActionMap m_BattleMenuParty;
    private IBattleMenuPartyActions m_BattleMenuPartyActionsCallbackInterface;
    private readonly InputAction m_BattleMenuParty_Move;
    private readonly InputAction m_BattleMenuParty_Select;
    private readonly InputAction m_BattleMenuParty_Cancel;
    public struct BattleMenuPartyActions
    {
        private @Controls m_Wrapper;
        public BattleMenuPartyActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BattleMenuParty_Move;
        public InputAction @Select => m_Wrapper.m_BattleMenuParty_Select;
        public InputAction @Cancel => m_Wrapper.m_BattleMenuParty_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_BattleMenuParty; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleMenuPartyActions set) { return set.Get(); }
        public void SetCallbacks(IBattleMenuPartyActions instance)
        {
            if (m_Wrapper.m_BattleMenuPartyActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BattleMenuPartyActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BattleMenuPartyActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BattleMenuPartyActionsCallbackInterface.OnMove;
                @Select.started -= m_Wrapper.m_BattleMenuPartyActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleMenuPartyActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleMenuPartyActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_BattleMenuPartyActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleMenuPartyActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleMenuPartyActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_BattleMenuPartyActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public BattleMenuPartyActions @BattleMenuParty => new BattleMenuPartyActions(this);

    // BattleMenuPartyCommand
    private readonly InputActionMap m_BattleMenuPartyCommand;
    private IBattleMenuPartyCommandActions m_BattleMenuPartyCommandActionsCallbackInterface;
    private readonly InputAction m_BattleMenuPartyCommand_Move;
    private readonly InputAction m_BattleMenuPartyCommand_Select;
    private readonly InputAction m_BattleMenuPartyCommand_Cancel;
    public struct BattleMenuPartyCommandActions
    {
        private @Controls m_Wrapper;
        public BattleMenuPartyCommandActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BattleMenuPartyCommand_Move;
        public InputAction @Select => m_Wrapper.m_BattleMenuPartyCommand_Select;
        public InputAction @Cancel => m_Wrapper.m_BattleMenuPartyCommand_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_BattleMenuPartyCommand; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleMenuPartyCommandActions set) { return set.Get(); }
        public void SetCallbacks(IBattleMenuPartyCommandActions instance)
        {
            if (m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface.OnMove;
                @Select.started -= m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_BattleMenuPartyCommandActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public BattleMenuPartyCommandActions @BattleMenuPartyCommand => new BattleMenuPartyCommandActions(this);

    // BattleMenuBag
    private readonly InputActionMap m_BattleMenuBag;
    private IBattleMenuBagActions m_BattleMenuBagActionsCallbackInterface;
    private readonly InputAction m_BattleMenuBag_Move;
    private readonly InputAction m_BattleMenuBag_Select;
    private readonly InputAction m_BattleMenuBag_Cancel;
    public struct BattleMenuBagActions
    {
        private @Controls m_Wrapper;
        public BattleMenuBagActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BattleMenuBag_Move;
        public InputAction @Select => m_Wrapper.m_BattleMenuBag_Select;
        public InputAction @Cancel => m_Wrapper.m_BattleMenuBag_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_BattleMenuBag; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleMenuBagActions set) { return set.Get(); }
        public void SetCallbacks(IBattleMenuBagActions instance)
        {
            if (m_Wrapper.m_BattleMenuBagActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BattleMenuBagActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BattleMenuBagActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BattleMenuBagActionsCallbackInterface.OnMove;
                @Select.started -= m_Wrapper.m_BattleMenuBagActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleMenuBagActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleMenuBagActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_BattleMenuBagActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleMenuBagActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleMenuBagActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_BattleMenuBagActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public BattleMenuBagActions @BattleMenuBag => new BattleMenuBagActions(this);

    // BattleMenuBagItem
    private readonly InputActionMap m_BattleMenuBagItem;
    private IBattleMenuBagItemActions m_BattleMenuBagItemActionsCallbackInterface;
    private readonly InputAction m_BattleMenuBagItem_Move;
    private readonly InputAction m_BattleMenuBagItem_Select;
    private readonly InputAction m_BattleMenuBagItem_Cancel;
    public struct BattleMenuBagItemActions
    {
        private @Controls m_Wrapper;
        public BattleMenuBagItemActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BattleMenuBagItem_Move;
        public InputAction @Select => m_Wrapper.m_BattleMenuBagItem_Select;
        public InputAction @Cancel => m_Wrapper.m_BattleMenuBagItem_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_BattleMenuBagItem; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleMenuBagItemActions set) { return set.Get(); }
        public void SetCallbacks(IBattleMenuBagItemActions instance)
        {
            if (m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface.OnMove;
                @Select.started -= m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_BattleMenuBagItemActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public BattleMenuBagItemActions @BattleMenuBagItem => new BattleMenuBagItemActions(this);

    // BattleMenuBagCommand
    private readonly InputActionMap m_BattleMenuBagCommand;
    private IBattleMenuBagCommandActions m_BattleMenuBagCommandActionsCallbackInterface;
    private readonly InputAction m_BattleMenuBagCommand_Move;
    private readonly InputAction m_BattleMenuBagCommand_Select;
    private readonly InputAction m_BattleMenuBagCommand_Cancel;
    public struct BattleMenuBagCommandActions
    {
        private @Controls m_Wrapper;
        public BattleMenuBagCommandActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BattleMenuBagCommand_Move;
        public InputAction @Select => m_Wrapper.m_BattleMenuBagCommand_Select;
        public InputAction @Cancel => m_Wrapper.m_BattleMenuBagCommand_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_BattleMenuBagCommand; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleMenuBagCommandActions set) { return set.Get(); }
        public void SetCallbacks(IBattleMenuBagCommandActions instance)
        {
            if (m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface.OnMove;
                @Select.started -= m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_BattleMenuBagCommandActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public BattleMenuBagCommandActions @BattleMenuBagCommand => new BattleMenuBagCommandActions(this);
    private int m_DefaultSchemeIndex = -1;
    public InputControlScheme DefaultScheme
    {
        get
        {
            if (m_DefaultSchemeIndex == -1) m_DefaultSchemeIndex = asset.FindControlSchemeIndex("Default");
            return asset.controlSchemes[m_DefaultSchemeIndex];
        }
    }
    public interface IBattleActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnSpecial(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnDisplayBattle(InputAction.CallbackContext context);
    }
    public interface IBattleDialogActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IBattleMenuCommandActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IBattleMenuFightActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnSpecial(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IBattleMenuFieldTargetActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IBattleMenuPartyActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IBattleMenuPartyCommandActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IBattleMenuBagActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IBattleMenuBagItemActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IBattleMenuBagCommandActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
}
