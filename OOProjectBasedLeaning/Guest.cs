using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OOProjectBasedLeaning
{

    public interface Guest : Model
    {

        void GoTo(Place place);

        Guest AddRoom(Room room);

        Guest RemoveRoom();

        Room StayAt();

        bool IsMember();

        bool IsVIP();
        //bool IsAtCheckIN();
        //bool IsAtCheckOUT();
    }

    public interface Member : Guest
    {

        const int NEW = 0;

        int Id { get; }

        bool IsNew();

    }

    public abstract class AbstractGuest : NotifierModelEntity, Guest
    {
        private Place place = NullPlace.Instance;
        private Room room = NullRoom.Instance;

        //private string status = "NONE"; // 状態管理用フィールド

        //public string Name { get; protected set; } = "未設定";

        
        //public bool IsAtCheckIN()//チェックインの判定
        //{
        //    return status == "IN";
        //}

        //public bool IsAtCheckOUT()//チェックアウトの判定
        //{
        //    return status == "OUT";
        //}

        //public void SetStatus(string status)
        //{
        //    this.status = status;
        //}

        public AbstractGuest()
        {

        }

        public AbstractGuest(string name)
        {

            Name = name;

        }

        public void GoTo(Place place) //CheckInかCheckOutのどちらかを行う
        {

            if (place is Home)//Hotelにいた場状態で、HomeにドロップされるときCheckOut
            {

                if (StayAt() is not NullObject)
                {
                    string message = $"支払額は{StayAt().Price:N0}円です。";
                    var result = MessageBox.Show(message, "ご精算", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.OK) //OKが押されたとき、チェックアウトする
                    {
                        StayAt().Hotel.CheckOut(this);
                        MessageBox.Show("チェックアウトが完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Cancelが押されたとき、チェックアウトをしない
                        MessageBox.Show("チェックアウトをキャンセルしました。", "中止", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }

            }
            else if (place is Hotel)//HotelにドロップされるときCheckIn
            {
                try
                {
                    (place as Hotel).CheckIn(this);
                }
                catch(AlreadyCheckedInException ex)
                {
                    MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }

            this.place = place;

            Notify();

        }

        public Guest AddRoom(Room room)
        {

            this.room = room;

            return this;

        }

        public Guest RemoveRoom()
        {

            room = NullRoom.Instance;

            return this;

        }

        public Room StayAt()
        {

            return room;

        }

        public abstract bool IsMember();

        public abstract bool IsVIP();

    }

    public class GuestModel : AbstractGuest
    {

        public GuestModel()
        {

        }

        public GuestModel(string name) : base(name)
        {

        }

        public override bool IsMember()
        {

            return false;

        }

        public override bool IsVIP()
        {

            return false;

        }

    }

    public class MemberModel : AbstractGuest, Member 
    {

        private int id;

        public MemberModel() : this(Member.NEW)
        {

        }

        public MemberModel(int id) : this(id, string.Empty)
        {

        }

        public MemberModel(string name) : this(Member.NEW, name)
        {

        }

        public MemberModel(int id, string name) : base(name)
        {

            this.id = id;

        }

        public override int GetHashCode()
        {

            return Id;

        }

        public override bool Equals(object? obj)
        {

            if (obj is Member)
            {

                return Id == (obj as Member)?.Id;

            }

            return false;

        }

        public int Id { get { return id; } }

        public bool IsNew()
        {

            return id is Member.NEW;

        }

        public override bool IsMember()
        {

            return true;

        }

        public override bool IsVIP()
        {

            // TODO: Implement VIP logic for members

            // Assuming members are not VIPs by default
            return false;

        }

    }

    public class NullGuest : AbstractGuest, NullObject
    {

        private static Guest instance = new NullGuest();

        private NullGuest() : base(string.Empty)
        {

        }

        public static Guest Instance { get { return instance; } }

        public override bool IsMember()
        {

            return false;

        }

        public override bool IsVIP()
        {

            return false;

        }

    }

}
