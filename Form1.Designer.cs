namespace ShapeMap
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.shapeView1 = new ShapeMap.shapeView();
            this.SuspendLayout();
            // 
            // shapeView1
            // 
            this.shapeView1.BackColor = System.Drawing.Color.White;
            this.shapeView1.Location = new System.Drawing.Point(251, 12);
            this.shapeView1.Name = "shapeView1";
            this.shapeView1.Size = new System.Drawing.Size(1194, 771);
            this.shapeView1.TabIndex = 0;
            this.shapeView1.Paint += new System.Windows.Forms.PaintEventHandler(this.shapeView1_Paint);
            this.shapeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.shapeView1_MouseClick);
            this.shapeView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.shapeView1_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1444, 795);
            this.Controls.Add(this.shapeView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private shapeView shapeView1;
    }
}

