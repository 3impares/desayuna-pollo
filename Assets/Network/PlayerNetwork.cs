using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    private readonly NetworkVariable<PlayerNetworkData> _netPos = new(writePerm: NetworkVariableWritePermission.Owner);
    private Vector3 _vel;
    private float _rotVel;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;
    
    void Update()
    {
        if (IsOwner)
        {
            _netPos.Value = new PlayerNetworkData()
            {
                Position = transform.position
            };
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, _netPos.Value.Position, ref _vel, _cheapInterpolationTime);
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _y;

        internal Vector3 Position
        {
            get => new Vector3(_x, _y, 0);
            set
            {
                _x = value.x;
                _y = value.y;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
        }
    }
}
