<template>
  <div class="mind">
    <el-select v-on:change="set_theme" v-model="theme_value" placeholder="主题">
      <el-option value="">default</el-option>
      <el-option value="primary">primary</el-option>
      <el-option value="warning">warning</el-option>
      <el-option value="danger">danger</el-option>
      <el-option value="success">success</el-option>
      <el-option value="info">info</el-option>
      <el-option value="greensea" selected="selected">greensea</el-option>
      <el-option value="nephrite">nephrite</el-option>
      <el-option value="belizehole">belizehole</el-option>
      <el-option value="wisteria">wisteria</el-option>
      <el-option value="asphalt">asphalt</el-option>
      <el-option value="orange">orange</el-option>
      <el-option value="pumpkin">pumpkin</el-option>
      <el-option value="pomegranate">pomegranate</el-option>
      <el-option value="clouds">clouds</el-option>
      <el-option value="asbestos">asbestos</el-option>
    </el-select>
    <div id="jsmind_container"></div>
    <div
        class="el-popover"
        :style="'left:'+pop.left+'px;top:'+pop.top+'px;'"
        v-show="pop.visible">
      <h1>hello</h1>
    </div>
  </div>
</template>
<script>

module.exports =  {
  name: "mind",
  data() {
    return {
      pop: {
        visible: false,
        top: 0,
        left: 0
      },
      direction: 'rtl',
      theme_value: '',
      mind: {
        meta: {
          name: 'jsMind remote',
          author: 'hizzgdev@163.com',
          version: '0.2'
        },
        format: 'node_tree',
        data: {
          id: 'root',
          topic: 'jsMind',
          children: [
            {
              id: 'easy',
              topic: 'Easy',
              direction: 'left',
              expanded: false,
              children: [
                {id: 'easy1', topic: 'Easy to show'},
                {id: 'easy2', topic: 'Easy to edit'},
                {id: 'easy3', topic: 'Easy to store'},
                {id: 'easy4', topic: 'Easy to embed'}
              ]
            },
            {
              id: 'open',
              topic: 'Open Source',
              direction: 'right',
              expanded: true,
              children: [
                {id: 'open1', topic: 'on GitHub'},
                {id: 'open2', topic: 'BSD License'}
              ]
            },
            {
              id: 'powerful',
              topic: 'Powerful',
              direction: 'right',
              children: [
                {id: 'powerful1', topic: 'Base on Javascript'},
                {id: 'powerful2', topic: 'Base on HTML5'},
                {id: 'powerful3', topic: 'Depends on you'}
              ]
            },
            {
              id: 'other',
              topic: 'test node',
              direction: 'left',
              children: [
                {id: 'other1', topic: "I'm from local variable"},
                {id: 'other2', topic: 'I can do everything'}
              ]
            }
          ]
        }
      },
      options: {
        container: 'jsmind_container', // [必选] 容器的ID
        editable: true, // [可选] 是否启用编辑
        theme: 'orange', // [可选] 主题
        shortcut: {
          enable: true,        // 是否启用快捷键
          handles: {},         // 命名的快捷键事件处理器
          mapping: {           // 快捷键映射
            addchild: 9,    // <Insert>
            addbrother: 13,    // <Enter>
            editnode: 32,   // <Space>
            delnode: 8,    // <Backspace>
            toggle: 113,    // <F2>
            left: 37,    // <Left>
            up: 38,    // <Up>
            right: 39,    // <Right>
            down: 40,    // <Down>
          }
        },
      }
    }
  },
  methods: {
    // 选择主题颜色
    set_theme() {
      this.jm.set_theme(this.theme_value)
    },
    // 获取选中标签的 ID
    get_selected_node_id() {
      console.log(this.jm);
      const selectedNode = this.jm.get_selected_node();
      if (selectedNode) {
        return selectedNode.id
      } else {
        return null
      }
    }
  },
  created() {
    const that = this;
    document.oncontextmenu = function (event) {
      const e = event||window.event;
      if (e.target.nodeName === "JMNODE") {
        that.pop.left = e.clientX;
        that.pop.top = e.clientY;
        that.pop.visible = true;
      }else{
        that.pop.visible = false;
      }
      return false;
    }
    document.onclick = function (){
      that.pop.visible = false;
    }
  },
  mounted() {
    this.jm = new jsMind(this.options);
    this.jm.show(this.mind);
    this.jm.enable_edit();
    this.jm.draggable = true;
  }
}
</script>

<style>
.mind{
  width: 100%;
  height: 100%;
  margin: 0;
  overflow: hidden;
}
.el-select{
  position: absolute;
  top: 0;
  left: 0;
  z-index: 10;
}
#jsmind_container{
  width: 100%;
  height: 100%;
}
</style>
