using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json;

namespace HBImageLabaler
{
    public partial class ImageLebeler : Form
    {
        private string SourcePath = "";
        private int currentIndex = 0;
        //private int x1, x2, y1, y2 = 0;
        private List<string> classes = new List<string>();

        string labeledImagesPath = "\\LabeledImages\\";
        string MergeFolder = "\\MergeFolder\\";
        string LabelsPath = "\\Labels\\";
        string OmitsPath = "\\Omits\\";
        string FinishedImagesPath = "\\FinishedImages\\";
        string WorkInProgressImagesPath = "\\WorkInProgressImages\\";

        string currentProjectPath = "";
        string projectName = "";
        Project _currentProject = new Project();
        Img ActiveImage = new Img();
        Image ActiveViewableImage;


        //For Drawing Marquerel
        Point startPos;      // mouse-down position
        Point currentPos;    // current mouse position
        bool drawing;        // busy drawing
        bool opening= false;        //Project Openning
        List<Rectangle> rectangles = new List<Rectangle>();  // previous rectangles

        private Rectangle getRectangle()
        {
            return new Rectangle(
                Math.Min(startPos.X, currentPos.X),
                Math.Min(startPos.Y, currentPos.Y),
                Math.Abs(startPos.X - currentPos.X),
                Math.Abs(startPos.Y - currentPos.Y));
        }
// marqurel code end

        public ImageLebeler()
        {
            InitializeComponent();
            ctxClassLebels.ItemAdded += new ToolStripItemEventHandler(context_added);
        }

        private void lstClassList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void lstClassList_DoubleClick(object sender, EventArgs e)
        {
            if(_currentProject.ID == null)
            { MessageBox.Show("Please Create a Project First.");
                return;
            }
            string input = Microsoft.VisualBasic.Interaction.InputBox("Input Class Name", "Add New Class", "", -1, -1);
            if (input != "")
            {
                lstClassList.Items.Add(input);
                if (_currentProject.Classes==null)
                {
                    _currentProject.Classes = new List<string>();
                }
                _currentProject.Classes.Add(input);

            }


            populateContextMenu(_currentProject.Classes);
            updateProjectJson();

        }

        private void populateContextMenu(List<string> classes)
        {
            ctxClassLebels.Items.Clear();
            foreach (string item in classes)
            {
                ctxClassLebels.Items.Add(item);
                

            }
        
        }

        private void context_added(object sender, ToolStripItemEventArgs e)
        {
            e.Item.Click += new EventHandler(label_clicked);
        }

        private void label_clicked(object sender, EventArgs e)
        {
            if (_currentProject.Images == null)
            {
                _currentProject.Images = new List<Img>();
            }

            if (ActiveImage.AnnotatedLabels == null)
            {
                ActiveImage.AnnotatedLabels = new List<ImgLabel>();
            }
            Rectangle last = rectangles.Last();
            ActiveImage.AnnotatedLabels.Add(new ImgLabel { X1 = last.Left, Y1 = last.Top, X2 = (last.Left + last.Width), Y2 = (last.Top + last.Height), Label = sender.ToString() });



           // updateProjectJson();

          
            
            txtOutput.Text += last.Left + ","
             +last.Top + ',' + (last.Left+last.Width) + ","
             + (last.Top + last.Height) + ',' + sender.ToString() + System.Environment.NewLine;
        }

        private void btnOpenImageFolder_Click(object sender, EventArgs e)
        {
            if (_currentProject.ID == null)
            {
                MessageBox.Show("Please Create a Project First.");
                return;
            }
            FolderBrowserDialog opener = new FolderBrowserDialog();
            //opener. = "Jpg|*.jpg |Jpeg |*.jpeg |Png |*.png ";
            DialogResult res =  opener.ShowDialog();
            if (!string.IsNullOrWhiteSpace(opener.SelectedPath))
            {
                OpenSourceFolder(opener.SelectedPath);
            }

        }

        private void OpenSourceFolder(string Path)
        {
            SourcePath = Path;
            List<string> files = Directory.EnumerateFiles(SourcePath).Where(s => s.EndsWith(".jpg") || s.EndsWith(".jpeg") || s.EndsWith(".png")).ToList();
            //
            foreach (var item in files)
            {

                clbImageList.Items.Add(item);

            }
            _currentProject.LastActiveSourceFolder = SourcePath;
            updateProjectJson();
            if (clbImageList.Items.Count > 0)
            {
                clbImageList.SetItemCheckState(0, CheckState.Checked);
            }
        }


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {

                ctxClassLebels.Show(e.X + splitContainer1.Left, e.Y + splitContainer1.Top);
            }

            if (drawing)
            {
                drawing = false;
                var rc = getRectangle();
                if (rc.Width > 0 && rc.Height > 0) rectangles.Add(rc);
                pictureBox1.Invalidate();
            }
        }

       

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Filter = "Labaling Project File | *.json";
            if (sDialog.ShowDialog() == DialogResult.OK)
            {
                string path = sDialog.FileName.Substring(0, sDialog.FileName.Count() - sDialog.FileName.Split('\\').Last().Count());
                string filename = sDialog.FileName.Split('\\').Last();
                path = path +  filename.Split('.')[0];
                Directory.CreateDirectory(path );
                

                Directory.CreateDirectory(path+ labeledImagesPath);
                Directory.CreateDirectory(path + MergeFolder);
                Directory.CreateDirectory(path + LabelsPath);
                Directory.CreateDirectory(path + OmitsPath);
                Directory.CreateDirectory(path + FinishedImagesPath);
                Directory.CreateDirectory(path + WorkInProgressImagesPath);

                
                filename = path + "\\" + filename;
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    //  sw.WriteLine("Hello World!");
                    Project project = new Project();
                    project.ID = Guid.NewGuid().ToString();
                    project.Name= sDialog.FileName.Split('\\').Last().Split('.')[0];
                    project.Path = path;
                    project.CreatedOn = DateTime.Now;
                    project.User = ConfigurationManager.AppSettings["User"];
                    string json = JsonConvert.SerializeObject(project);

                    sw.WriteLine(json);
                    currentProjectPath = path;
                    projectName = project.Name;
                    
                }
                OpenProject(filename);

            }

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opDialog = new OpenFileDialog();
            opDialog.Filter = "Labaling Project File | *.json";
            if (opDialog.ShowDialog() == DialogResult.OK)
            {
                opening = true;
                
                OpenProject(opDialog.FileName);
            }
        }

        private void OpenProject(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                _currentProject = JsonConvert.DeserializeObject<Project>(json);
                _currentProject.Path = filePath.Substring(0, filePath.Count() - filePath.Split('\\').Last().Count());
               
            }
            updateProjectJson();

            lblCurrentProject.Text = _currentProject.Name;
            if (_currentProject.Classes != null)
            {
                lstClassList.Items.Clear();
                foreach (var item in _currentProject.Classes)
                {
                    // classes.Add(item);
                    lstClassList.Items.Add(item);
                }
                populateContextMenu(_currentProject.Classes);
            }
            //open last folder
            if (_currentProject.LastActiveSourceFolder != null)
            {
                OpenSourceFolder(_currentProject.LastActiveSourceFolder);
                PopulateLastUsedSourceImages();

            }
            mergeToolStripMenuItem.Enabled = true;
            exportToolStripMenuItem.Enabled = true;

            

        }

        private void PopulateLastUsedSourceImages()
        {
            if (_currentProject.Images == null)
            {
                return;
            }
            int imgIndex = 0;
            for (imgIndex = 0; imgIndex < clbImageList.Items.Count;)
            {


                if (_currentProject.Images.Any(i => i.OriginalName == clbImageList.GetItemText(clbImageList.Items[imgIndex])))
                {
                    clbImageList.SetItemCheckState(imgIndex, CheckState.Checked);
                    if (imgIndex + 1 < clbImageList.Items.Count)
                    {
                        //opening = false;
                        clbImageList.SetItemCheckState(imgIndex + 1, CheckState.Checked);
                    }

                }
                imgIndex++;
                opening = false;

            }
        }

        private void clbImageList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void clbImageList_ItemCheck(object sender, ItemCheckEventArgs e)
        {

            string selectedimagepath =clbImageList.GetItemText(clbImageList.Items[e.Index]);
            currentIndex = e.Index;
            try
            {

                pictureBox1.Load(selectedimagepath);
                ActiveViewableImage = Image.FromFile(selectedimagepath);
               

                ActiveImage = new Img();
                ActiveImage.Id = Guid.NewGuid().ToString();
                ActiveImage.OriginalName = selectedimagepath;

                rectangles.Clear();
                try
                {


                    if (_currentProject.Images.Any(i => i.OriginalName == selectedimagepath))
                    {
                        ActiveImage = _currentProject.Images.Single(i => i.OriginalName == selectedimagepath);
                        List<ImgLabel> labels = ActiveImage.AnnotatedLabels;
                        foreach (var item in labels)
                        {
                            rectangles.Add(new Rectangle(item.X1, item.Y1, item.X2 - item.X1, item.Y2 - item.Y1));
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("No Image Selected");

                }
            }
            catch (Exception ex1)
            {

                Console.WriteLine("No Image Data");
            }
           
          


            //Img rec = new Img();
            //_currentProject.Records.Add()
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            CopyAnnotatedFileandUpdateModel();
            updateProjectJson();
            if (clbImageList.Items.Count > 0 && clbImageList.Items.Count > currentIndex + 1)
            {
               
                clbImageList.SetItemCheckState(currentIndex + 1, CheckState.Checked);
            }
           

        }

        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project toImport = new Project();
            string projectName = Microsoft.VisualBasic.Interaction.InputBox("Input Project Name", "Project Name to Merge", _currentProject.Name, -1, -1);
            if(projectName != "")
            {
                using (StreamReader r = new StreamReader(_currentProject.Path + MergeFolder + projectName + ".json"))
                {
                    string json = r.ReadToEnd();
                    toImport = JsonConvert.DeserializeObject<Project>(json);
                }

                if (toImport.Name == projectName)
                {
                    foreach (Img image in toImport.Images)
                    {
                        if (!_currentProject.Images.Any(i => i.Id == image.Id))
                        {
                            //copy
                            string filename = image.Id + "." + image.OriginalName.Split('.')[1];
                            Image imagetocopy = Image.FromFile(_currentProject.Path + MergeFolder + labeledImagesPath + filename);
                            imagetocopy.Save(_currentProject.Path + labeledImagesPath + filename);

                            _currentProject.Images.Add(image);
                            updateProjectJson();
                            //


                        }
                    }
                    MessageBox.Show("Merge Done");
                }
            }
           

        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(_currentProject.Path + "\\" + LabelsPath+_currentProject.Name + ".csv"))
            {
                StringBuilder sb = new StringBuilder();
              
                //  sw.WriteLine("Hello World!");
                foreach (Img img in _currentProject.Images)
                {
                    if (img.AnnotatedLabels != null)
                    {
                        foreach (ImgLabel item in img.AnnotatedLabels)
                        {
                            sb.AppendLine(string.Join(",", new[] { img.Id+"."+img.OriginalName.Split('.')[1],
                                                    img.Width.ToString(),img.Height.ToString(),
                                                    item.Label, item.X1.ToString(),item.Y1.ToString(),
                                                    item.X2.ToString(),item.Y2.ToString()}));
                        } 
                    }
                    //else
                    //{
                    //    image = Image.FromFile(img.OriginalName);
                    //    image.Save(_currentProject.Path + OmitsPath + ActiveImage.OriginalName.Split('\\').Last());


                    //}

                }
               




                sw.WriteLine(sb.ToString());


            }
            Image image;
            List<Img> NullAnnotations = _currentProject.Images.Where(i => i.AnnotatedLabels == null).ToList();
            foreach (Img item in NullAnnotations)
            {
                image = Image.FromFile(item.OriginalName);
                image.Save(_currentProject.Path + OmitsPath + item.OriginalName.Split('\\').Last());

            }
            MessageBox.Show("Export Done.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mergeToolStripMenuItem.Enabled = false;
            exportToolStripMenuItem.Enabled = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            currentPos = e.Location;
            if (drawing) pictureBox1.Invalidate();


            

            }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!opening)
            {
                if (rectangles.Count > 0) e.Graphics.DrawRectangles(Pens.Black, rectangles.ToArray());

            }
            
            if (drawing) e.Graphics.DrawRectangle(Pens.Red, getRectangle());
        }

        private void CopyAnnotatedFileandUpdateModel()
        {
            if (!_currentProject.Images.Any(i => i.Id == ActiveImage.Id))

            {
                Image image = Image.FromFile(ActiveImage.OriginalName);
                image.Save(_currentProject.Path + labeledImagesPath + ActiveImage.Id + "." + ActiveImage.OriginalName.Split('.')[1]);
                ActiveImage.Height = image.Height;
                ActiveImage.Width = image.Width;

                _currentProject.Images.Add(ActiveImage);
            }

        }

        private void updateProjectJson()
        {
            using (StreamWriter sw = new StreamWriter(_currentProject.Path + "\\"+_currentProject.Name + ".json"))
            {
                //  sw.WriteLine("Hello World!");
               ;
                string json = JsonConvert.SerializeObject(_currentProject);

                sw.WriteLine(json);
               

            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            currentPos = startPos = e.Location;
            drawing = true;
        }
    }

   
}
