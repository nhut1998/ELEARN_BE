using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DecentralizedSystem.Models.Buy
{
    public class AccountTranFccModel
    {
        [JsonProperty("ma_cif")]
        public string MaCif { get; set; }

        [JsonProperty("ho_ten")]
        public string HoTen { get; set; }

        [JsonProperty("cmnd")]
        public string cmnd { get; set; }

        [JsonProperty("stk")]
        public string Stk { get; set; }

        [JsonProperty("loai_tien")]
        public string LoaiTien { get; set; }

        [JsonProperty("loai_hinh")]
        public string LoaiHinh { get; set; }

        [JsonProperty("lai_suat")]
        public decimal LaiSuat { get; set; }

        [JsonProperty("hinh_thuc_linh_lai")]
        public string HinhThucLinhLai { get; set; }

        [JsonProperty("ngay_mo")]
        public DateTime NgayMo { get; set; }

        [JsonProperty("ngay_tai_ky")]
        public DateTime NgayTaiKy { get; set; }

        [JsonProperty("ngay_dao_han")]
        public DateTime NgayDaoHan { get; set; }

        [JsonProperty("ngay_cn_gan_nhat")]
        public DateTime NgayCnGanNhat { get; set; }

        [JsonProperty("so_ngay_nam_giu")]
        public decimal SoNgayNamGiu { get; set; }

        [JsonProperty("giatri_giao_dich")]
        public decimal GiaTriGiaoDich { get; set; }

        [JsonProperty("phi_chuyen_nhuong")]
        public decimal PhiChuyenNhuong { get; set; }

        [JsonProperty("so_tien_kh_nhan_cn")]
        public decimal SoTienKhNhanCn { get; set; }

        [JsonProperty("so_tien_kh_tt_nhan_cn")]
        public decimal SoTienKHTTNhanCN { get; set; }

        [JsonProperty("trang_thai")]
        public string TrangThai { get; set; }

        [JsonProperty("ma_san_pham")]
        public string MaSanPham { get; set; }

        [JsonProperty("so_seri")]
        public string SoSeri { get; set; }

        [JsonProperty("so_du")]
        public decimal SoDu { get; set; }

        [JsonProperty("so_ngay_con_lai")]
        public int SoNgayConLai { get; set; }

        [JsonProperty("ngaytl_gannhat")]
        public DateTime NgayTLGanNhat { get; set; }

        [JsonProperty("lai_duoc_huong")]
        public decimal LaiDuocHuong { get; set; }

        [JsonProperty("don_vi_mo")]
        public string DonViMo { get; set; }

        [JsonProperty("phan_tram_phi_chuyen_nhuong")]
        public decimal PhanTramPhiChuyenNhuong { get; set; }

        [JsonProperty("nguoi_dai_dien")]
        public string NguoiDaiDien { get; set; }
        [JsonProperty("loai_kh")]
        public string LoaiKh { get; set; }

        [JsonProperty("hinhthuc_daohan")]
        public string HinhThucDaoHan { get; set; }

        [JsonProperty("khoan_vay_phai_thu_hoi")]
        public decimal KhoanVayPhaiThuHoi { get; set; }

        [JsonProperty("lai_da_linh_truoc")]
        public decimal LaiDaLinhTruoc { get; set; }

        [JsonProperty("ds_tai_khoan_vay")]
        public dynamic DSTaiKhoanVay { get; set; }

        [JsonProperty("custom_field_data")]
        public string CustomFieldData { get; set; }

        [JsonProperty("tk_nhan_lai")]
        public string TkNhanLai { get; set; }

        [JsonProperty("tk_nhan_goc")]
        public string TkNhanGoc { get; set; }
    }

    public class AccountTranReportModel
    {
        [JsonProperty("maturity_date")]
        public List<AccountTranFccModel> MaturityDate { get; set; }

        [JsonProperty("success_data")]
        public List<AccountTranFccModel> SuccessData { get; set; }

        [JsonProperty("active_data")]
        public List<AccountTranFccModel> ActiveDate { get; set; } 
    }


    public class AccountTranReportsModel
    {
        [JsonProperty("stk")]
        public string Stk { get; set; }

        [JsonProperty("loai_hinh")]
        public string LoaiHinh { get; set; }
        [JsonProperty("ngay_mo")]
        public DateTime NgayMo { get; set; }
        [JsonProperty("ngay_mo_str")]
        public string NgayMoStr { get; set; }

        [JsonProperty("ngay_den_han_str")]
        public string NgayDenHanStr { get; set; }

        [JsonProperty("ngay_tai_ky")]
        public DateTime NgayTaiKy { get; set; }
        [JsonProperty("so_du")]
        public decimal SoDu { get; set; }
        [JsonProperty("so_du_str")]
        public string SoDuStr { get; set; }
        [JsonProperty("giatri_giao_dich")]
        public decimal GiaTriGiaoDich { get; set; }
        [JsonProperty("giatri_giao_dich_str")]
        public string GiaTriGiaoDichStr { get; set; }

    }

    public class ReportTranEndModel
    {
        [JsonProperty("maturity_date")]
        public List<AccountTranReportsModel> MaturityDate { get; set; }

        [JsonProperty("success_data")]
        public List<AccountTranReportsModel> SuccessData { get; set; }

        [JsonProperty("active_data")]
        public List<AccountTranReportsModel> ActiveDate { get; set; }

        [JsonProperty("sum_maturity")]
        public string SumMaturity  { get; set; }

        [JsonProperty("sum_abc")]
        public string SumABC { get; set; }

        [JsonProperty("sum_success")]
        public string SumSuccess { get; set; }

        [JsonProperty("sum_active")]
        public string SumActive { get; set; }

        [JsonProperty("export_date")]
        public DateTime ExportDate { get; set; }

        [JsonProperty("export_date_str")]
        public string ExportDateStr { get; set; }
    }
}
