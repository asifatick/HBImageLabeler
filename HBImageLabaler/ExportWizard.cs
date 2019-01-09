using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HBImageLabaler
{
    public partial class ExportWizard : Form
    {
        public ImageLebeler mainForm;
        Hashtable groups = new Hashtable();
        Dictionary<string, bool> selectedImages = new Dictionary<string, bool>();
        public ExportWizard()
        {
            
            InitializeComponent();
        }

        private void ExportWizard_Load(object sender, EventArgs e)
        {
           // mainForm.splitToolStripMenuItem_Click(sender, e);
            lstImages.BeginUpdate();
            
            foreach (var item in mainForm._currentProject.Classes)
            {
                // classes.Add(item);
                clbClassList.Items.Add(item, true);
                lstImages.Groups.Add(item, item);
              
                if (!groups.Contains(item))
                {
                    groups.Add(item, new ListViewGroup(item,
                        HorizontalAlignment.Left));
                }
                LoadImages(mainForm._currentProject.Path+ "Labels\\" + item+"\\", item);
            }
            lstImages.LargeImageList = imageList;
            lstImages.EndUpdate();

        }

        private void LoadImages( string folderPath, string group)
        {
            if (Directory.Exists(folderPath))
            {
                List<string> files = Directory.EnumerateFiles(folderPath).Where(s => s.EndsWith(".jpg") || s.EndsWith(".jpeg") || s.EndsWith(".png")).ToList();
                //
                foreach (var item in files)
                {


                    addImage(item, group);
                }
            }
        }
        private void addImage(string imageToLoad, string group)
        {
            if (imageToLoad != "")
            {
                string id = imageToLoad.Split('\\').Last().Split('_')[1].Split('.')[0];
                string order = imageToLoad.Split('\\').Last().Split('_')[0];
                string key = id + "_" + order;
                imageList.Images.Add(key,Image.FromFile(imageToLoad));
                //lstImages.BeginUpdate();
                ListViewItem li = new ListViewItem();
                li.Text = key;
                
                li.ToolTipText = imageToLoad;
                li.ImageKey = key;
                li.Group = lstImages.Groups[group];
                li.Checked = true;
                lstImages.Items.Add(li);
                //lstImages.EndUpdate();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstImages.Items)
            {
                item.Checked = chkSelectAll.Checked;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //set a hashtable with selected classes
            Dictionary<string,bool> selectedClasses = new Dictionary<string, bool>();
            int index = 0;
            foreach (var item in clbClassList.Items)
            {
                if(clbClassList.GetItemChecked(index))
                {
                    selectedClasses.Add(item.ToString(), true);
                }
                else
                {
                    selectedClasses.Add(item.ToString(), false);
                }
                index++;
            }
            using (StreamWriter sw = new StreamWriter(mainForm._currentProject.Path + "\\" + mainForm.LabelsPath + mainForm._currentProject.Name + ".csv"))
            {
                StringBuilder sb = new StringBuilder();

                //  sw.WriteLine("Hello World!");
                sb.AppendLine(string.Join(",", new[] { "filename",
                                                    "width","height",
                                                    "class", "xmin","ymin",
                                                    "xmax","ymax"}));
                foreach (Img img in mainForm._currentProject.Images)
                {
                    if (img.AnnotatedLabels != null)
                    {
                        foreach (ImgLabel item in img.AnnotatedLabels)
                        {
                            if(selectedClasses[item.Label] )
                            {
                                if(selectedImages[img.Id+"_"+item.Id])
                                {
                                    //label indec removed (mainForm._currentProject.Classes.IndexOf(item.Label)+1).ToString() ,
                                    sb.AppendLine(string.Join(",", new[] { img.Id+"."+img.OriginalName.Split('.')[1],
                                                    img.Width.ToString(),img.Height.ToString(),
                                                    item.Label,  item.X1.ToString(),item.Y1.ToString(),
                                                    item.X2.ToString(),item.Y2.ToString()}));
                                }
                            }
                            
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
            List<Img> NullAnnotations = mainForm._currentProject.Images.Where(i => i.AnnotatedLabels == null).ToList();
            foreach (Img item in NullAnnotations)
            {
                image = Image.FromFile(item.OriginalName);
                image.Save(mainForm._currentProject.Path + mainForm.OmitsPath + item.OriginalName.Split('\\').Last());

            }
            MessageBox.Show("Export Done.");
        }

        private void lstImages_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!selectedImages.ContainsKey(e.Item.ImageKey))
            {
                selectedImages.Add(e.Item.ImageKey, true);
            }

            if (e.Item.Checked == true)
            {
                selectedImages[e.Item.ImageKey] =true;
            }
            else
            {
                selectedImages[e.Item.ImageKey] = false;
            }
        }
    }
 }

