using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace Cyriller.Desktop.Models
{
    public class CyrCollectionContainer
    {
        public bool IsInitialized { get; private set; }
        private object Locker { get; set; } = new object();
        private Task InitTask { get; set; }

        public virtual CyrNounCollection CyrNounCollection { get; protected set; }
        public virtual CyrAdjectiveCollection CyrAdjectiveCollection { get; protected set; }

        public void InitCollectionsInBackground()
        {
            lock (Locker)
            {
                if (IsInitialized)
                {
                    return;
                }

                this.InitTask = this.InitCollections();
            }
        }

        public async ValueTask InitOrDefault()
        {
            if (this.IsInitialized)
            {
                return;
            }

            await this.InitTask;
        }

        protected virtual Task InitCollections() => Task.Run(() =>
        {
            lock (Locker)
            {
                if (IsInitialized)
                {
                    return;
                }

                this.CyrNounCollection = new CyrNounCollection();
                this.CyrAdjectiveCollection = new CyrAdjectiveCollection();
                this.IsInitialized = true;
            }
        });
    }
}
