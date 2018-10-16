using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.ReadersRight;
using LibflClassLibrary.Readers.ReadersRights;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LibflClassLibrary.Controls.Readers
{
    public partial class fReaderRegistrationAndRights : Form
    {
        public fReaderRegistrationAndRights()
        {
            InitializeComponent();
        }
        ReaderInfo reader;
        public void Init(int NumberReader)
        {
            reader = ReaderInfo.GetReader(NumberReader);
            label1.Text = "Регистрационные данные читателя №" + reader.NumberReader + ".\n " + reader.FamilyName + " " + reader.Name + " " + reader.FatherName;

            DataTable CountryTable = ReaderInfo.GetReaderCountries();
            cbCountry.DataSource = CountryTable;
            cbCountry.ValueMember = "IDCountry";
            cbCountry.DisplayMember = "NameCountry";
            cbCountry.SelectedValue = reader.RegistrationCountry;

            tbRegion.Text = reader.RegistrationRegion;
            tbProvince.Text = reader.RegistrationProvince;
            tbDistrict.Text = reader.RegistrationDistrict;
            tbCity.Text = reader.RegistrationCity;
            tbStreet.Text = reader.RegistrationStreet;
            tbHouse.Text = reader.RegistrationHouse;
            tbFlat.Text = reader.RegistrationFlat;
            if (reader.MobileTelephone.Length == 14)
            {
                tbMobilePhoneCode.Text = reader.MobileTelephone.Substring(3, 3);
                tbMobilePhone.Text = reader.MobileTelephone.Substring(7, 7);
            }
            tbEmail.Text = reader.Email;
            readerRightsView1.Init(NumberReader);

            DisableAll();
        }

        private bool AllEnabled = false;
        private void DisableAll()
        {
            cbCountry.Enabled = false;
            tbMobilePhone.Enabled = false;
            tbMobilePhoneCode.Enabled = false;
            tbCity.Enabled = false;
            tbDistrict.Enabled = false;
            tbFlat.Enabled = false;
            tbHouse.Enabled = false;
            tbProvince.Enabled = false;
            tbRegion.Enabled = false;
            tbStreet.Enabled = false;
            tbEmail.Enabled = false;
            bSave.Enabled = false;
            bEdit.Enabled = true;
            AllEnabled = false;
        }
        private void EnableAll()
        {
            cbCountry.Enabled = true;
            tbMobilePhone.Enabled = true;
            tbMobilePhoneCode.Enabled = true;
            tbCity.Enabled = true;
            tbDistrict.Enabled = true;
            tbFlat.Enabled = true;
            tbHouse.Enabled = true;
            tbProvince.Enabled = true;
            tbRegion.Enabled = true;
            tbStreet.Enabled = true;
            bSave.Enabled = true;
            tbEmail.Enabled = true;
            bEdit.Enabled = false;
            AllEnabled = true;
        }

        private void bGiveFreeAbonementRight_Click(object sender, EventArgs e)
        {
            if (AllEnabled)
            {
                MessageBox.Show("Выйдите их режима редактирования! Нажмите кнопку \"Сохранить\"!");
                return;
            }
            if (reader.Rights[ReaderRightsEnum.FreeAbonement] != null)
            {
                MessageBox.Show("Права бесплатного абонемента уже выданы!");
                return;
            }
            if (reader.RegistrationCountry == 1 || reader.RegistrationCity == "" || reader.RegistrationStreet == "" ||
                reader.RegistrationHouse == "" || reader.RegistrationFlat == "" || reader.MobileTelephone == "") 
            {
                MessageBox.Show("Перед выдачей прав необходимо обязательно указать все поля, отмеченные звёздочкой!");
                return;
            }

            TimeSpan Age = (DateTime.Now - reader.DateBirth);
            DateTime zeroTime = new DateTime(1, 1, 1);
            int Years = (zeroTime + Age).Year - 1;
            if (Years < 14)
            {
                MessageBox.Show("Бесплатный абонемент не выдаётся до достижения 14 летнего возраста!");
                return;
            }
            reader.Rights.GiveFreeAbonementRight();
            reader = ReaderInfo.GetReader(reader.NumberReader);
            this.Init(reader.NumberReader);
            MessageBox.Show("Права бесплатного абонемента успешно выданы!");
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (tbMobilePhone.Text.Length > 14)
            {
                MessageBox.Show("Поле Мобильный телефон не может содержать более 14 символов!");
                return;
            }
            if (tbEmail.Text != string.Empty
                &&
                !Regex.IsMatch(tbEmail.Text,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"))
            {
                MessageBox.Show("Поле Email имеет неверный формат!");
                return;
            }
            if (tbMobilePhone.Text == string.Empty
                ||
                tbMobilePhoneCode.Text == string.Empty)
            {
                MessageBox.Show("Мобильный телефон не заполнен полностью!");
                return;
            }
            if (tbMobilePhoneCode.Text.Length != 3)
            {
                MessageBox.Show("Код мобильного телефона должен содержать 3 цифры!");
                return;
            }
            if (tbMobilePhone.Text.Length != 7)
            {
                MessageBox.Show("Номер мобильного телефона должен содержать 7 цифр!");
                return;
            }
            if (!int.TryParse(tbMobilePhoneCode.Text, out int ParsedPhone))
            {
                MessageBox.Show("Код мобильного телефона имеет неверный формат!");
                return;
            }
            if (!int.TryParse(tbMobilePhone.Text, out ParsedPhone))
            {
                MessageBox.Show("Код мобильного телефона имеет неверный формат!");
                return;
            }


            reader.RegistrationCity = tbCity.Text;
            reader.RegistrationCountry = (int)cbCountry.SelectedValue;
            reader.RegistrationDistrict = tbDistrict.Text;
            reader.RegistrationFlat = tbFlat.Text;
            reader.RegistrationHouse = tbHouse.Text;
            reader.RegistrationProvince = tbProvince.Text;
            reader.RegistrationRegion = tbRegion.Text;
            reader.RegistrationStreet = tbStreet.Text;
            reader.MobileTelephone = (tbMobilePhoneCode.Text == string.Empty) ? string.Empty : $"+7({tbMobilePhoneCode.Text}){tbMobilePhone.Text}";
            reader.Email = tbEmail.Text;

            reader.UpdateRegistrationFields();
            DisableAll();
            reader = ReaderInfo.GetReader(reader.NumberReader);
            MessageBox.Show("Регистрационные данные обновлены");
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            EnableAll();
        }
    }
}
