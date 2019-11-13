namespace HKLiteDemo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdateByCondition = new System.Windows.Forms.Button();
            this.btnUpdateByKey = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnQuerySql = new System.Windows.Forms.Button();
            this.btnQueryCustomerCondition = new System.Windows.Forms.Button();
            this.btnQueryTop = new System.Windows.Forms.Button();
            this.btnQueryByCondition = new System.Windows.Forms.Button();
            this.btnQueryByKey = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnDeleteByKey = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnTrans3 = new System.Windows.Forms.Button();
            this.btnTrans2 = new System.Windows.Forms.Button();
            this.btnTrans1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.btnQueryColumns = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUpdateByCondition);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnUpdateByKey);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(163, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "新增/更新";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 33);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(128, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "插入记录";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdateByCondition
            // 
            this.btnUpdateByCondition.Location = new System.Drawing.Point(16, 91);
            this.btnUpdateByCondition.Name = "btnUpdateByCondition";
            this.btnUpdateByCondition.Size = new System.Drawing.Size(128, 23);
            this.btnUpdateByCondition.TabIndex = 2;
            this.btnUpdateByCondition.Text = "按条件更新";
            this.btnUpdateByCondition.UseVisualStyleBackColor = true;
            this.btnUpdateByCondition.Click += new System.EventHandler(this.btnUpdateByCondition_Click);
            // 
            // btnUpdateByKey
            // 
            this.btnUpdateByKey.Location = new System.Drawing.Point(16, 62);
            this.btnUpdateByKey.Name = "btnUpdateByKey";
            this.btnUpdateByKey.Size = new System.Drawing.Size(128, 23);
            this.btnUpdateByKey.TabIndex = 1;
            this.btnUpdateByKey.Text = "按主键更新";
            this.btnUpdateByKey.UseVisualStyleBackColor = true;
            this.btnUpdateByKey.Click += new System.EventHandler(this.btnUpdateByKey_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnQueryColumns);
            this.groupBox3.Controls.Add(this.btnQuerySql);
            this.groupBox3.Controls.Add(this.btnQueryCustomerCondition);
            this.groupBox3.Controls.Add(this.btnQueryTop);
            this.groupBox3.Controls.Add(this.btnQueryByCondition);
            this.groupBox3.Controls.Add(this.btnQueryByKey);
            this.groupBox3.Location = new System.Drawing.Point(181, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(325, 136);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "查询";
            // 
            // btnQuerySql
            // 
            this.btnQuerySql.Location = new System.Drawing.Point(173, 91);
            this.btnQuerySql.Name = "btnQuerySql";
            this.btnQuerySql.Size = new System.Drawing.Size(128, 23);
            this.btnQuerySql.TabIndex = 6;
            this.btnQuerySql.Text = "SQL查询";
            this.btnQuerySql.UseVisualStyleBackColor = true;
            this.btnQuerySql.Click += new System.EventHandler(this.btnQuerySql_Click);
            // 
            // btnQueryCustomerCondition
            // 
            this.btnQueryCustomerCondition.Location = new System.Drawing.Point(173, 62);
            this.btnQueryCustomerCondition.Name = "btnQueryCustomerCondition";
            this.btnQueryCustomerCondition.Size = new System.Drawing.Size(128, 23);
            this.btnQueryCustomerCondition.TabIndex = 5;
            this.btnQueryCustomerCondition.Text = "自定义条件查询";
            this.btnQueryCustomerCondition.UseVisualStyleBackColor = true;
            this.btnQueryCustomerCondition.Click += new System.EventHandler(this.btnQueryCustomerCondition_Click);
            // 
            // btnQueryTop
            // 
            this.btnQueryTop.Location = new System.Drawing.Point(20, 91);
            this.btnQueryTop.Name = "btnQueryTop";
            this.btnQueryTop.Size = new System.Drawing.Size(128, 23);
            this.btnQueryTop.TabIndex = 4;
            this.btnQueryTop.Text = "查询前N条数据";
            this.btnQueryTop.UseVisualStyleBackColor = true;
            this.btnQueryTop.Click += new System.EventHandler(this.btnQueryTop_Click);
            // 
            // btnQueryByCondition
            // 
            this.btnQueryByCondition.Location = new System.Drawing.Point(19, 62);
            this.btnQueryByCondition.Name = "btnQueryByCondition";
            this.btnQueryByCondition.Size = new System.Drawing.Size(128, 23);
            this.btnQueryByCondition.TabIndex = 3;
            this.btnQueryByCondition.Text = "按条件查询";
            this.btnQueryByCondition.UseVisualStyleBackColor = true;
            this.btnQueryByCondition.Click += new System.EventHandler(this.btnQueryByCondition_Click);
            // 
            // btnQueryByKey
            // 
            this.btnQueryByKey.Location = new System.Drawing.Point(19, 33);
            this.btnQueryByKey.Name = "btnQueryByKey";
            this.btnQueryByKey.Size = new System.Drawing.Size(128, 23);
            this.btnQueryByKey.TabIndex = 2;
            this.btnQueryByKey.Text = "按主键查询";
            this.btnQueryByKey.UseVisualStyleBackColor = true;
            this.btnQueryByKey.Click += new System.EventHandler(this.btnQueryByKey_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnDeleteByKey);
            this.groupBox4.Controls.Add(this.btnDelete);
            this.groupBox4.Controls.Add(this.btnDeleteAll);
            this.groupBox4.Location = new System.Drawing.Point(512, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(169, 136);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "删除";
            // 
            // btnDeleteByKey
            // 
            this.btnDeleteByKey.Location = new System.Drawing.Point(20, 31);
            this.btnDeleteByKey.Name = "btnDeleteByKey";
            this.btnDeleteByKey.Size = new System.Drawing.Size(128, 23);
            this.btnDeleteByKey.TabIndex = 3;
            this.btnDeleteByKey.Text = "按主键删除";
            this.btnDeleteByKey.UseVisualStyleBackColor = true;
            this.btnDeleteByKey.Click += new System.EventHandler(this.btnDeleteByKey_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(20, 62);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(128, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "按条件删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Location = new System.Drawing.Point(20, 91);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(128, 23);
            this.btnDeleteAll.TabIndex = 1;
            this.btnDeleteAll.Text = "删除所有数据";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnTrans3);
            this.groupBox5.Controls.Add(this.btnTrans2);
            this.groupBox5.Controls.Add(this.btnTrans1);
            this.groupBox5.Location = new System.Drawing.Point(687, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(163, 136);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "事务";
            // 
            // btnTrans3
            // 
            this.btnTrans3.Location = new System.Drawing.Point(19, 91);
            this.btnTrans3.Name = "btnTrans3";
            this.btnTrans3.Size = new System.Drawing.Size(128, 23);
            this.btnTrans3.TabIndex = 5;
            this.btnTrans3.Text = "事务操作3";
            this.btnTrans3.UseVisualStyleBackColor = true;
            this.btnTrans3.Click += new System.EventHandler(this.btnTrans3_Click);
            // 
            // btnTrans2
            // 
            this.btnTrans2.Location = new System.Drawing.Point(19, 62);
            this.btnTrans2.Name = "btnTrans2";
            this.btnTrans2.Size = new System.Drawing.Size(128, 23);
            this.btnTrans2.TabIndex = 4;
            this.btnTrans2.Text = "事务操作2";
            this.btnTrans2.UseVisualStyleBackColor = true;
            this.btnTrans2.Click += new System.EventHandler(this.btnTrans2_Click);
            // 
            // btnTrans1
            // 
            this.btnTrans1.Location = new System.Drawing.Point(19, 31);
            this.btnTrans1.Name = "btnTrans1";
            this.btnTrans1.Size = new System.Drawing.Size(128, 23);
            this.btnTrans1.TabIndex = 3;
            this.btnTrans1.Text = "事务操作1";
            this.btnTrans1.UseVisualStyleBackColor = true;
            this.btnTrans1.Click += new System.EventHandler(this.btnTrans1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 154);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(838, 317);
            this.dataGridView1.TabIndex = 5;
            // 
            // btnQueryColumns
            // 
            this.btnQueryColumns.Location = new System.Drawing.Point(173, 33);
            this.btnQueryColumns.Name = "btnQueryColumns";
            this.btnQueryColumns.Size = new System.Drawing.Size(128, 23);
            this.btnQueryColumns.TabIndex = 7;
            this.btnQueryColumns.Text = "查询指定列";
            this.btnQueryColumns.UseVisualStyleBackColor = true;
            this.btnQueryColumns.Click += new System.EventHandler(this.btnQueryColumns_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 483);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HKLiteDemo";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdateByCondition;
        private System.Windows.Forms.Button btnUpdateByKey;
        private System.Windows.Forms.Button btnTrans1;
        private System.Windows.Forms.Button btnTrans2;
        private System.Windows.Forms.Button btnTrans3;
        private System.Windows.Forms.Button btnQueryByKey;
        private System.Windows.Forms.Button btnQueryByCondition;
        private System.Windows.Forms.Button btnQuerySql;
        private System.Windows.Forms.Button btnQueryCustomerCondition;
        private System.Windows.Forms.Button btnQueryTop;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button btnDeleteByKey;
        private System.Windows.Forms.Button btnQueryColumns;
    }
}

