namespace ByteShop.Catalog.Domain.Entities
{
    public class Produto
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public string ImagemUrl { get; private set; }
        public DateTime DataCriacao { get; private set; }

  
        protected Produto() { }

       
        public Produto(string nome, string descricao, decimal preco, int quantidadeEstoque, string imagemUrl)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
            ImagemUrl = imagemUrl;
            DataCriacao = DateTime.UtcNow;

            Validar();
        }

        public void AtualizarEstoque(int novaQuantidade)
        {
            if (novaQuantidade < 0)
                throw new ArgumentException("A quantidade em estoque não pode ser negativa.");

            QuantidadeEstoque = novaQuantidade;
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new ArgumentException("O nome do produto é obrigatório.");

            if (Preco <= 0)
                throw new ArgumentException("O preço deve ser maior que zero.");
        }
    }
}