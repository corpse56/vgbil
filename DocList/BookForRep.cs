using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocList
{
    class BookForRep
    {
        public BookForRep(Book b)
        {
            string title = ((b.author != null) ? b.author : "");
            title += (title != "") ? ((b.title != null) ? ", " + b.title : b.title) : b.title;
            string size = ((b.volume != null) ? b.volume : "");
            size += (size != "") ? ((b.illustrs != null) ? ", " + b.illustrs : b.illustrs) : b.illustrs;
            size += (size != "") ? ((b.size != null) ? ", " + b.size : b.size) : b.size;
            string pub = ((b.placepub != null) ? b.placepub : "");
            pub += (pub != "") ? ((b.pubhouse != null) ? ": " + b.pubhouse : b.pubhouse) : b.pubhouse;
            string note = ((b.note != null) ? b.note : "");
            note += (note != "") ? ((b.notesp != null) ? ". " + b.notesp : b.notesp) : b.notesp;
            this.idm = b.idm;
            this.title = title;
            this.pubhouse = pub;
            this.dpublish = b.dpublish;
            this.volume = size;
            this.note = note;
            this.inv = b.inv;
            this.cdc = b.cdc;
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
    }
}
