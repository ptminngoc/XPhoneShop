using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.Repositories;

namespace XPhone_Shop_TKPM.ViewModels
{
    class AddNewPromotionViewModel
    {
        private AddNewPromotionRepository _repo;

        public AddNewPromotionViewModel()
        {
            _repo = new AddNewPromotionRepository();
        }

        public void addNewPromo(PromotionModel newPromo)
        {
            _repo.addNewPromo(newPromo);
        }
    }
}
