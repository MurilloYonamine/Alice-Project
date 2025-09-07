using UnityEngine;
using PLAYER.INPUT;

namespace PLAYER.MOBILE
{
    /// <summary>
    /// Script exemplo de como configurar o player para funcionar corretamente com mobile
    /// Adicione este script ao GameObject do Player junto com os outros componentes
    /// </summary>
    public class MobilePlayerSetup : MonoBehaviour
    {
        
        [Header("Ground Detection")]
        [SerializeField] private LayerMask groundLayer = 1; // Layer do chão
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckRadius = 0.1f;
        
        private void Start()
        {
            ConfigureForMobile();
        }

        private void ConfigureForMobile()
        {
            // Configura input para mobile
            var inputManager = FindFirstObjectByType<InputPlatformManager>();
            if (inputManager == null)
            {
                // Cria o input manager automaticamente
                var inputManagerObj = new GameObject("Input Platform Manager");
                inputManager = inputManagerObj.AddComponent<InputPlatformManager>();
                
                // Adiciona detector de double tap
                inputManagerObj.AddComponent<TouchJumpDetector>();
                
            }

            // Verifica configuração do Rigidbody2D
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Configurações recomendadas para evitar que o player "caia em buracos"
                rb.freezeRotation = true; // Evita rotação indesejada
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                rb.gravityScale = 1f; // Gravidade normal
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Desenha a área de detecção do chão
            if (groundCheckPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
            }
        }

        /// <summary>
        /// Método para verificar se o player está no chão
        /// Pode ser usado para debug do problema de "sempre caindo"
        /// </summary>
        public bool IsGrounded()
        {
            if (groundCheckPoint == null) return false;
            
            return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        }
    }
}
