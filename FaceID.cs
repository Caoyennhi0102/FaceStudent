using AForge.Video;
using AForge.Video.DirectShow;
using FaceStudent.DataAccess;
using FaceStudent.Model;
using OpenCvSharp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace FaceStudent
{
    public partial class FaceID : Form
    {
        private SplitContainer splitContainer;
        private PictureBox cameraBox;
        private DataGridView attendanceList;
        private FilterInfoCollection videoDevices; // Danh sách các thiết bị video
        private VideoCaptureDevice videoSource;   // Nguồn video đang sử dụng
        private CascadeClassifier faceDetector;   // Dùng cho nhận diện khuôn mặt
        private VideoCapture cameraCapture;
        private Timer timer;
        private bool isCameraRunning = false;
        private Button btnExit;
        private StudentRepository _StudentRepository;


   
       
        public FaceID()
        {
            InitializeComponent();
            InitializeUI();
            _StudentRepository = new StudentRepository();
            InitializeCamera();
        }

        private void InitializeCamera()
        {
            

            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy thiết bị camera!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Chọn camera đầu tiên
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
                // Tải Haar cascade XML để phát hiện khuôn mặt
                string haarFile = "haarcascade_frontalface_default.xml";
                if (!File.Exists(haarFile))
                {
                    MessageBox.Show("Không tìm thấy file Haar cascade!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                faceDetector = new CascadeClassifier(haarFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo camera: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            videoSource.Start();
        }
        // Dừng camera khi thoát form
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
            base.OnFormClosing(e);
        }
        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
                Mat matFrame = BitmapToMat(frame);

                // Phát hiện khuôn mặt
                var faces = faceDetector.DetectMultiScale(
                    matFrame,
                    scaleFactor: 1.1,
                    minNeighbors: 6,
                    flags: HaarDetectionTypes.ScaleImage,
                    minSize: new OpenCvSharp.Size(50, 50));

                using (Graphics g = Graphics.FromImage(frame))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    foreach (var face in faces)
                    {
                        Rectangle faceRect = new Rectangle(face.X, face.Y, face.Width, face.Height);
                        g.DrawRectangle(Pens.Green, faceRect);
                    }
                    if (faces.Length > 0)
                    {
                        //string studentID;
                        // Lấy tọa độ khuôn mặt 
                        //var faceRect = faces[0];
                        // Tạo FaceID từ tọa độ khuôn mặt 
                        //string recognizedFaceId = $"Face_{faceRect.X}_{faceRect.Y}_{faceRect.Width}_{faceRect.Height}";
                        // Gọi StudentRepository để truy xuất học sinh  
                        int studdentID = 1;
                        var student = _StudentRepository.GetStudentById(studdentID);

                        if(student != null) {

                           Invoke(new Action(() =>
                            {
                                attendanceList.Rows.Add(new object[] { student.StudentID, student.StudentName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
    
                            }));
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy học sinh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                cameraBox.Invoke((MethodInvoker)delegate
                {
                    cameraBox.Image?.Dispose();
                    cameraBox.Image = frame;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xử lý khung hình: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Mat BitmapToMat(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                return Cv2.ImDecode(ms.ToArray(), ImreadModes.Color);
            }
        }
        private void StopCamera()
        {
            try
            {
                timer.Stop();
                cameraCapture?.Release();
                cameraBox.Image?.Dispose();
                isCameraRunning = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi dừng camera: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeUI()
        {
            this.Text = "Hệ Thống Điểm Danh";
            this.WindowState = FormWindowState.Maximized;

            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = System.Windows.Forms.Orientation.Vertical,
                SplitterDistance = (int)(this.Width * 0.7),
            };
            var rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
            };
            splitContainer.Panel2.Controls.Add(rightPanel);
            var buttonPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.LightGray
            };
            rightPanel.Controls.Add(buttonPanel);
            this.Controls.Add(splitContainer);
            // Tạo nút Thoát
            btnExit = new Button
            {
                Text = "Thoát",
                Location = new System.Drawing.Point(120, 10)  // Sử dụng Point thay vì System.Drawing.Size
            };
            btnExit.Click += BtnExit_Click; // Gọi sự kiện khi nhấn nút Thoát
            buttonPanel.Controls.Add(btnExit);



            cameraBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            splitContainer.Panel1.Controls.Add(cameraBox);
            attendanceList = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = this.Height,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
            };
            attendanceList.Columns.Add("ID", "Mã Học Sinh");
            attendanceList.Columns.Add("Name", "Họ và Tên");
            attendanceList.Columns.Add("Time", "Ngày và Giờ");
            splitContainer.Panel2.Controls.Add(attendanceList);
        }
        // Sự kiện khi nhấn nút Thoát
private void BtnExit_Click(object sender, EventArgs e)
{
    // Đóng form hiện tại
    this.Close();

    // Mở form HomeCheckin
    HomeCheckin homeCheckinForm = new HomeCheckin();
    homeCheckinForm.Show();
}


    }
}
