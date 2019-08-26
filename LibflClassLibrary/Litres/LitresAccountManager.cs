using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Litres
{
    public class LitresAccountManager
    {
        


        LitresLoader loader = new LitresLoader();
        public LitresInfo GetLitresAccount(int ReaderId)
        {
            LitresInfo result = loader.GetLitresAccount(ReaderId);
            if (result == null)
            {
                throw new Exception("L001");
            }
            return result;
        }
        public LitresInfo AssignLitresAccount(int ReaderId)
        {
            LitresInfo result = loader.GetLitresAccount(ReaderId);
            if (result != null)
            {
                throw new Exception("L002");
            }
            loader.AssignLitresAccount(ReaderId);
            result = loader.GetLitresAccount(ReaderId);
            return result;
        }

        public LitresInfo GetLitresNewAccount()
        {
            LitresApiHandler api = new LitresApiHandler();
            string sid = api.w_create_sid();
            LitresInfo result = api.w_biblio_reader_create(sid);
            this.InsertNewLitresAccount(result);
            return result;
        }

        public void InsertNewLitresAccount(LitresInfo newAccount)
        {
            loader.InsertNewLitresAccount(newAccount);
        }


    }
}
