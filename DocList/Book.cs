using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocList
{
    class Book
    {
        public Book()
        {
        }
        public Book(BooksView b)
        {
            this.author = b.author;
            this.bar = b.bar;
            this.cdc = b.cdc;
            this.dpublish = b.dpublish;
            this.id = b.id;
            this.idm = b.idm;
            this.illustrs = b.illustrs;
            this.inv = b.inv;
            this.note = b.note;
            this.notesp = b.notesp;
            this.placepub = b.placepub;
            this.pubhouse = b.pubhouse;
            this.size = b.size;
            this.title = b.title;
            this.volume = b.volume;
        }
        public Book(BooksViewRED b)
        {
            this.author = b.author;
            this.bar = b.bar;
            this.cdc = b.cdc;
            this.dpublish = b.dpublish;
            this.id = b.id;
            this.idm = b.idm;
            this.illustrs = b.illustrs;
            this.inv = b.inv;
            this.note = b.note;
            this.notesp = b.notesp;
            this.placepub = b.placepub;
            this.pubhouse = b.pubhouse;
            this.size = b.size;
            this.title = b.title;
            this.volume = b.volume;
        }
        public Book(BooksViewFCC b)
        {
            this.author = b.author;
            this.bar = b.bar;
            this.cdc = b.cdc;
            this.dpublish = b.dpublish;
            this.id = b.id;
            this.idm = b.idm;
            this.illustrs = b.illustrs;
            this.inv = b.inv;
            this.note = b.note;
            this.notesp = b.notesp;
            this.placepub = b.placepub;
            this.pubhouse = b.pubhouse;
            this.size = b.size;
            this.title = b.title;
            this.volume = b.volume;
        }
        public int idm
        {
            get
            {
                return this._idm;
            }
            set
            {
                this._idm = value;
            }
        }
        private int _idm;
        public string title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }
        private string _title;
        public string author
        {
            get
            {
                return this._author;
            }
            set
            {
                this._author = value;
            }
        }
        private string _author;
        public string pubhouse
        {
            get
            {
                return this._pubhouse;
            }
            set
            {
                this._pubhouse = value;
            }
        }
        private string _pubhouse;
        public string placepub
        {
            get
            {
                return this._placepub;
            }
            set
            {
                this._placepub = value;
            }
        }
        private string _placepub;
        public string dpublish
        {
            get
            {
                return this._dpublish;
            }
            set
            {
                this._dpublish = value;
            }
        }
        private string _dpublish;
        public string volume
        {
            get
            {
                return this._volume;
            }
            set
            {
                this._volume = value;
            }
        }
        private string _volume;
        public string illustrs
        {
            get
            {
                return this._illustrs;
            }
            set
            {
                this._illustrs = value;
            }
        }
        private string _illustrs;
        public string size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
            }
        }
        private string _size;
        public string note
        {
            get
            {
                return this._note;
            }
            set
            {
                this._note = value;
            }
        }
        private string _note;
        public string notesp
        {
            get
            {
                return this._notesp;
            }
            set
            {
                this._notesp = value;
            }
        }
        private string _notesp;
        public string inv
        {
            get
            {
                return this._inv;
            }
            set
            {
                this._inv = value;
            }
        }
        private string _inv;
        public string cdc
        {
            get
            {
                return this._cdc;
            }
            set
            {
                this._cdc = value;
            }
        }
        private string _cdc;
        public string bar
        {
            get
            {
                return this._bar;
            }
            set
            {
                this._bar = value;
            }
        }
        private string _bar;
        public int? id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }
        private  int? _id;
    }

}
