using System.Collections.Generic;

namespace PriceCollector.View.SliderMenu
{
    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {

            this.Add(new MenuItem()
            {
                Titulo = "Produtos Coletados",
                TargetType = typeof(MainPage)
            });
            
            this.Add(new MenuItem()
            {
                Titulo = "Produtos Alvo",
                TargetType = typeof(TargetProductsPage)
            });
            
            this.Add(new MenuItem()
            {
                Titulo = "Cadastros de Supermercados",
                TargetType = typeof(SupermarketsCompetitorsPage)
            });

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
