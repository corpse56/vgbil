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
            tbMobilePhone.Text = reader.MobileTelephone;

            readerRightsView1.Init(NumberReader);

            DisableAll();
        }

        private bool AllEnabled = false;
        private void DisableAll()
        {
            cbCountry.Enabled = false;
            tbMobilePhone.Enabled = false;
            tbCity.Enabled = false;
            tbDistrict.Enabled = false;
            tbFlat.Enabled = false;
            tbHouse.Enabled = false;
            tbProvince.Enabled = false;
            tbRegion.Enabled = false;
            tbStreet.Enabled = false;
            bSave.Enabled = false;
            bEdit.Enabled = true;
            AllEnabled = false;
        }
        private void EnableAll()
        {
            cbCountry.Enabled = true;
            tbMobilePhone.Enabled = true;
            tbCity.Enabled = true;
            tbDistrict.Enabled = true;
            tbFlat.Enabled = true;
            tbHouse.Enabled = true;
            tbProvince.Enabled = true;
            tbRegion.Enabled = true;
            tbStreet.Enabled = true;
            bSave.Enabled = true;
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
            if (reader.RegistrationCountry == 1 || reader.RegistrationCity == "")
            {
                MessageBox.Show("Перед выдачей прав необходимо обязательно указать страну и город!");
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

            reader.RegistrationCity = tbCity.Text;
            reader.RegistrationCountry = (int)cbCountry.SelectedValue;
            reader.RegistrationDistrict = tbDistrict.Text;
            reader.RegistrationFlat = tbFlat.Text;
            reader.RegistrationHouse = tbHouse.Text;
            reader.RegistrationProvince = tbProvince.Text;
            reader.RegistrationRegion = tbRegion.Text;
            reader.RegistrationStreet = tbStreet.Text;
            reader.MobileTelephone = tbMobilePhone.Text;


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
