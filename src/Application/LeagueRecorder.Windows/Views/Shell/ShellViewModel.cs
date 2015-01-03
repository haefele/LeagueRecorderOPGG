using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Storage;
using LiteGuard;
using ReactiveUI;

namespace LeagueRecorder.Windows.Views.Shell
{
    public class ShellViewModel : ReactiveScreen
    {
        #region Fields
        private readonly IUserStorage _userStorage;

        private ObservableAsPropertyHelper<ReactiveObservableCollection<User>> _users;
        private User _selectedUser;
        #endregion

        #region Properties
        public ReactiveCommand<ReactiveObservableCollection<User>> LoadUsers { get; private set; }
        public ReactiveCommand<object> NewUser { get; private set; }
        public ReactiveCommand<object> DeleteUser { get; private set; } 

        public ReactiveObservableCollection<User> Users
        {
            get { return this._users.Value; }
        }
        public User SelectedUser
        {
            get { return this._selectedUser; }
            set { this.RaiseAndSetIfChanged(ref this._selectedUser, value); }
        }

        #endregion

        public ShellViewModel(IUserStorage userStorage)
        {
            Guard.AgainstNullArgument("userStorage", userStorage);

            this._userStorage = userStorage;

            this.CreateCommands();
        }

        protected override async void OnInitialize()
        {
            await this.LoadUsers.ExecuteAsyncTask();
        }

        private  void CreateCommands()
        {
            this.LoadUsers = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var users = await this._userStorage.GetUsersAsync();

                var result = new ReactiveObservableCollection<User>();
                result.AddRange(users);
                return result;
            });
            this.LoadUsers.ToProperty(this, f => f.Users, out this._users);

            this.NewUser = ReactiveCommand.Create();
            this.NewUser.Subscribe(_ =>
            {
                var newUser = new User();
                this.Users.Add(newUser);

                this.SelectedUser = newUser;
            });

            this.DeleteUser = ReactiveCommand.Create(this.WhenAny(f => f.SelectedUser, f => f.Value != null));
            this.DeleteUser.Subscribe(async _ =>
            {
                await this._userStorage.DeleteUserAsync(this.SelectedUser);
                this.Users.Remove(this.SelectedUser);
            });
        }
    }
}