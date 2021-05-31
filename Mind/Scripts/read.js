const app = new Vue ({
    el: '.read',
    components:{
        'mind': httpVueLoader("../../Content/mind.vue")
    },
    data() {
        return {
            activeIndex: '1',
            hasLogin: false,
            state:"",
            section:{},
            danmu_drawer: {
                visibility: false,
                direction: 'rtl',
                content: "",
                selected_text: ""
            },
            category_drawer:{
                visibility:false,
                direction:"ltr"
            },
            pop: {
                left: 0,
                top: 0,
                visibility: false
            },
            book:{},
            spans: [],
            underline_text: "下划线",
            span_target: null,
            mind_drawer: {
                visibility: false,
                direction: 'rtl'
            },
            
            danmuList:[],
            danmuData:[],
            show_danmu_flag:false,
            range:null,
            dm:null,
            light_flag:true
        }
    },
    methods: {
        select(e) {
            let selectedText = window.getSelection().toString();   // 把选中的内容转为字符串。
            if (selectedText.trim() !== "") {    // 去掉字符两边的空白
                //判断选中的文本是否已经下划线
                console.log(e.target);
                this.pop.left = e.pageX;
                this.pop.top = e.pageY;
                this.pop.visibility = true;
                this.danmu_drawer.selected_text = selectedText.trim();
                this.range = this.saveSelectionRange();
            }
        },
        change_theme(){
            if (this.light_flag){
                document.body.style.setProperty('--themeBodyBackground', '#393737');
                document.body.style.setProperty('--themeContentBackground', '#393737');
                document.body.style.setProperty('--themeColor', "#b7b0a1");
                document.body.style.setProperty('--themeBtnColor', "#fff");
                document.body.style.setProperty('--themeBtnBackground', "#909399");
            }else{
                document.body.style.setProperty('--themeBodyBackground', '#eeeded');
                document.body.style.setProperty('--themeContentBackground', '#eeeded');
                document.body.style.setProperty('--themeColor', "#000");
                document.body.style.setProperty('--themeBtnColor', "#000");
                document.body.style.setProperty('--themeBtnBackground', "#fff");
            }
            this.light_flag = !this.light_flag;
        },
        handleClose(done) {
            done();
            document.getSelection().removeAllRanges();
        },
        handleMindClose(done) {
            done();
            document.getSelection().removeAllRanges();
        },
        underline() {
            if (this.underline_text === "取消下划线") {
                console.log("取消");
                this.span_target.outerHTML = this.span_target.innerHTML;
            } else {
                let selectStr;
                if (this.range!=null) {
                    selectStr = this.danmu_drawer.selected_text
                    console.log(selectStr);
                    if (selectStr.trim() !== "") {
                        const range = this.range;
                        const span = document.createElement('span');
                        {
                            span.id = this.getRandomCode();
                            span.style.textDecoration = "underline";
                            span.style.textDecorationColor = "#4e9aff";
                            span.style.textDecorationStyle = "dashed";
                            span.style.textUnderlinePosition = "under";
                            span.style.cursor = "pointer";
                            span.style.userSelect = "none";
                        }
                        var that = this;
                        span.onclick = function (e) {
                            console.log(e.target);
                            that.pop.left = e.pageX;
                            that.pop.top = e.pageY;
                            that.pop.visibility = true;
                            that.underline_text = "取消下划线";
                            that.span_target = e.target;
                        }
                        this.spans.push(span.id);
                        span.appendChild(range.extractContents());
                        range.insertNode(span);
                    }
                }
            }
        },
        send_danmu() {
            //验证是否登录
            if (localStorage.getItem("email")==""){
                this.$confirm('未登录?', '是否前往登录', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    window.location.href = "/Home/Login"
                }).catch(() => {
                });
                return;
            }
            //在数据库中插入弹幕信息
            const that = this;
            axios({
                method: 'get',
                url: "/Read/AddDanmu?data="+JSON.stringify({
                    id: this.section.id,
                    email:localStorage.getItem("email"),
                    selected_text: this.danmu_drawer.selected_text,
                    content:  this.danmu_drawer.content
                })
            })
                .then(res => {
                    console.log(res.data);
                    that.get_danmu();
                })
                .catch(err => {
                    console.log(err);
                });
            //关闭drawer
            this.danmu_drawer.visibility = false;
            //取消选中
            document.getSelection().removeAllRanges();
            //关闭操作条
            this.pop.visibility = false;
        },
        
        getRandomCode(length) {
            if (length > 0) {
                const data = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"];
                let nums = "";
                for (let i = 0; i < length; i++) {
                    const r = parseInt(Math.random() * 61);
                    nums += data[r];
                }
                return nums;
            } else {
                return false;
            }
        },
        saveSelectionRange() {
            if (window.getSelection) {
                let sel = window.getSelection();
                if (sel.rangeCount > 0)
                    return sel.getRangeAt(0);
            } else if (document.selection) {
                let sel = document.selection;
                return sel.createRange();
            }
            return null;
        },
        setSelectionRange(savedSel) {
            if (!savedSel)
                return;
            if (window.getSelection) {
                var sel = window.getSelection();
                sel.removeAllRanges();
                sel.addRange(savedSel);
            } else if (document.selection) {
                savedSel.select();
            }
        },
        go(name,section){
            localStorage.setItem("section",JSON.stringify(section));
            this.get_section();
        },
        go_comment(){
            localStorage.setItem("section",JSON.stringify(this.section));
            localStorage.setItem("book",JSON.stringify(this.book));
            window.open("/Home/detail");
        },
        preSection(){
            let i=0;
            for (let section of this.book.sections) {
                if (section.id==this.section.id){
                    if (i==0) {
                        this.$message("已经是第一章");
                    }else{
                        localStorage.setItem("section",JSON.stringify(this.book.sections[i-1]))
                        this.get_section();
                    }
                }
                i++;
            }
        },
        nextSection(){
            let i=0;
            console.log(this.book.sections);
            console.log(this.section);
            for (let section of this.book.sections) {
                if (section.id==this.section.id){
                    console.log(section)
                    if (i==this.book.sections.length-1){
                        this.$message("已经是最后一章");
                    }else{
                        localStorage.setItem("section",JSON.stringify(this.book.sections[i+1]))
                        this.get_section();
                    }
                }
                i++;
            }
        },
        get_section(){
            const book = JSON.parse(localStorage.getItem("book"));
            const section = JSON.parse(localStorage.getItem("section"));
            let url = "";
            console.log(section);
            if (section==null){
                url = '/Read/GetSection?b_id='+book.id
            }else if (section["content"]===""){
                url = '/Read/GetSection?b_id='+book.id+'&s_id='+section.id;
            }
            const that = this;
            const loading = this.$loading({
                lock: true,
                text: 'Loading',
                spinner: 'el-icon-loading',
                background: 'rgba(0, 0, 0, 0.7)'
            });
            axios({
                method: 'get',
                url: url
            })
                .then(res => {
                    console.log(res.data);
                    that.section = res.data;
                    loading.close();
                    //获取弹幕
                    that.get_danmu();
                    document.getElementsByClassName("read")[0].scrollTop = 0;
                })
                .catch(err => {
                    console.log(err);
                });
        },
        get_danmu(){
            const that = this;
            axios({
                method: 'get',
                url: "/Read/GetDanmu?s_id="+that.section.id
            })
                .then(res => {
                    console.log(res.data);
                    res.data.forEach(function(item) {
                        let flag = true;
                        for (const itemElement of that.danmuList) {
                            if (itemElement.id == item.id){
                                flag = false;
                            }
                        }
                        if (flag){
                            that.danmuList.push(item)
                            that.danmuData.push(item.content)
                            // that.dm.send(item.content)
                        }
                    })
                    //实例化弹幕
                    that.dm = new danMu({
                        device:'pc',
                        comments: that.danmuData,
                        danMuHtml: "<span>{comment}</span>" ,
                        colors: ["#fff","#00ccff","#ff0000","#00ff00","#fef500"],
                        flyTime:7000,
                        timeInterval:1200,
                        randomSort:true
                    });
                })
                .catch(err => {
                    console.log(err);
                });
        },
        
        get_category(){
            const book = JSON.parse(localStorage.getItem("book"));
            const that = this;
            axios({
                method: 'get',
                url: "/Detail/GetBook?b_id="+book.id
            })
                .then(res => {
                    that.book = res.data;
                })
                .catch(err => {
                    console.log(err);
                });
        },
        isInShelf(){

        }
    },
    created() {
        this.isInShelf()
        //获取章节信息
        this.get_section();
        //获取目录信息
        this.get_category();
    }
});

function show_danmu(){
    console.log(app)
    if (app.$data.show_danmu_flag){
        $(".danmustage").hide();
    }else{
        $(".danmustage").show();
    }
    app.$data.show_danmu_flag = !app.$data.show_danmu_flag
}