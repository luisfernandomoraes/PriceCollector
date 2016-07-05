using System.Collections.Generic;

namespace PriceCollector.View.SliderMenu
{
    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            this.Add(new MenuItem()
                {
                    Titulo = "Lista de Compras",
                    TargetType = typeof(object)
                });

            //this.Add(new MenuItem()
            //    {
            //        Titulo = "Consulta de Preços",
            //        TargetType = typeof(SearchProduct)
            //    });

            //this.Add(new MenuItem()
            //    {
            //        Titulo = "Carrinho de Compras",
            //        TargetType = typeof(View.Cart)
            //    });

            //this.Add(new MenuItem()
            //    {
            //        Titulo = "Oferta de Produtos",
            //        TargetType = typeof(Promotions)
            //    });

            //this.Add(new MenuItem()
            //    {
            //        Titulo = "Avaliação da Loja",
            //        TargetType = typeof(StarControl)
            //    });

            //this.Add(new MenuItem()
            //    {
            //        Titulo = "Receitas Culinárias",
            //        TargetType = typeof(View.Recipe)
            //    });
            //this.Add(new MenuItem()
            //    {
            //        Titulo = "Contato",
            //        TargetType = typeof(ContactMarket)
            //    });
            //this.Add(new MenuItem()
            //    {
            //        Titulo = "Sobre o Aplicativo",
            //        TargetType = typeof(AboutApp)
            //    });
            //this.Add(new MenuItem()
            //{
            //    Titulo = "Atualização de Cadastro",
            //    TargetType = typeof(CreateUser)
            //});
            //this.Add(new MenuItem
            //    {
            //        Titulo = "Propriedades",
            //        TargetType = typeof(PropertiesPage)
            //    });
        }
    }
}
