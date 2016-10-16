
/*
        private  Task<List<Notification>> StorageUpdateNotifications()
        {
            Monitor.Enter(storageLoker);
            try
            {
                List<Notification> items =  _storage.GetAllItems<Notification>().Result;
                return Task.Run(() =>
                {
                    return items;
                });
            }
            catch(Exception ex)
            {
                string s = ex.Message;
                return Task.Run(() =>
                {
                    return new List<Notification>();
                });
            }
            finally
            {
                Monitor.Exit(storageLoker);
            }

        }


        private Task<List<Notification>> HttpUpdateNotifications()
        {

           
            List<Notification> items;

            string response = HttpGetMessage().Result;

            if (response != null)
            {
                items = ParseNotifications(response);


                if (items.Count > 0)
                {
                    items = SearChchangetFields(items);
                    if (items.Count > 0)
                    {

                        //
                        Task.Delay(5000).Wait();
                        //

                        Monitor.Enter(storageLoker);
                        try
                        {
                            int j;
                            foreach (Notification i in items)
                             j=    _storage.InsertOrReplaceItem<Notification>(i).Result;

                            return Task.Run(() =>
                            {
                                return items;
                            });
                        }
                        finally
                        {
                            Monitor.Exit(storageLoker);

                        }
                    }
                }

             


              
            }
            return Task.Run(() =>
            {
                return new List<Notification>();
            });
        }


        public async void GetNotifications()
        {

             Task<Task<List<Notification>>> StorageUpdateTask = new Task<Task<List<Notification>>>(async () =>
            {
                return await StorageUpdateNotifications();
            });

            StorageUpdateTask.Start();

            Task taskUI1 = StorageUpdateTask.ContinueWith(async (t) =>
            {
                 List < Notification > i = await t.Result;
                if (i.Count>0)
                    UpdatingNotification(this, new MessageEvent("", i));
            }, TaskScheduler.FromCurrentSynchronizationContext());



            Task<Task<List<Notification>>> HttpUpdateTask = new Task<Task<List<Notification>>>(async () =>
            {
                return await HttpUpdateNotifications();
            });

            HttpUpdateTask.Start();

            Task taskUI2 = HttpUpdateTask.ContinueWith(async (t) =>
            {
                List<Notification> i = await t.Result;
                if (i.Count > 0)
                    UpdatingNotification(this, new MessageEvent("", i));
            }, TaskScheduler.FromCurrentSynchronizationContext());


        }


        public async void ReadNotifications(Notification item)
        {
            item.State = true;
            Monitor.Enter(storageLoker);
            try
            {
              await  _storage.UpdateItem<Notification>(item);
            }
            finally
            {
                Monitor.Exit(storageLoker);
            }
        }
    }
}
*/