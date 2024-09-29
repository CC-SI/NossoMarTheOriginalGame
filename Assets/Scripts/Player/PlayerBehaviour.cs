using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        // Lista de patos seguindo o jogador
        readonly List<Duck> ducks = new();

        // Instância singleton da classe PlayerBehaviour
        public static PlayerBehaviour Instance { get; private set; }

        public Transform GetFollowTarget(Duck duck)
        {
            // Obtém o alvo a ser seguido
            Transform target = GetFollowTarget();

            // Adiciona o pato à lista se ainda não estiver nela
            if (!ducks.Contains(duck))
                ducks.Add(duck);

            return target;
        }

        public Transform GetFollowTarget()
        {
            // Se não há patos, retorna a posição do jogador
            if (ducks.Count < 1)
                return transform;

            // Retorna a posição do último pato na lista
            return ducks[^1].transform;
        }

        void Awake()
        {
            // Configura a instância singleton
            if (!Instance)
            {
                Instance = this;
                return;
            }

            Destroy(gameObject);
        }
    }
}