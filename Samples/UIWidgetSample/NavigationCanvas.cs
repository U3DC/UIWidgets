using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace UIWidgetsSample {
    
    public class NavigationCanvas : WidgetCanvas {
        
        protected override string initialRoute => "/";

        protected override Dictionary<string, WidgetBuilder> routes => new Dictionary<string, WidgetBuilder> {
            {"/", (context) => new HomeScreen()},
            {"/detail", (context) => new DetailScreen()}
        };
        
        protected override TextStyle textStyle => new TextStyle(fontSize: 24);
        
        protected override PageRouteFactory pageRouteBuilder => (RouteSettings settings, WidgetBuilder builder) =>
            new PageRouteBuilder(
                settings: settings,
                pageBuilder: (BuildContext context, Unity.UIWidgets.animation.Animation<double> animation, 
                    Unity.UIWidgets.animation.Animation<double> secondaryAnimation) => builder(context),
                transitionsBuilder: (BuildContext context, Animation<double> 
                    animation, Animation<double> secondaryAnimation, Widget child) => new _FadeUpwardsPageTransition(
                        routeAnimation: animation,
                        child: child
                    )
            );
    }
    
    class HomeScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: new Color(0xFF888888),
                child: new Center(child: new CustomButton(onPressed: () => {
                        Navigator.pushName(context, "/detail");
                    }, child: new Text("Go to Detail"))
                ));

        }
    }
    
    class DetailScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: new Color(0xFF1389FD),
             
                child: new Center(
                    child: new Column(
                        children: new List<Widget>() {
                            new CustomButton(onPressed: () => {
                                Navigator.pop(context);
                            }, child: new Text("Back")),
                            new CustomButton(onPressed: () => {
                                _Dialog.showDialog(context, builder: (BuildContext c) => new Dialog());
                            }, child: new Text("Show Dialog"))
                        }
                    )
            ));
        }
    }
    
    class Dialog : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Center(child:new Container(
                color: new Color(0xFFFF0000),
                width: 100,
                height: 80,
                child: new Center(
                    child: new Text("Hello Dialog")
                )));
        }
    }

    class _FadeUpwardsPageTransition : StatelessWidget {

        internal _FadeUpwardsPageTransition(
            Key key = null,
            Animation<double> routeAnimation = null, // The route's linear 0.0 - 1.0 animation.
            Widget child = null
        ) :base(key: key) {
            this._positionAnimation = _bottomUpTween.chain(_fastOutSlowInTween).animate(routeAnimation);
            this._opacityAnimation = _easeInTween.animate(routeAnimation);
            this.child = child;
        }

        static Tween<Offset> _bottomUpTween = new OffsetTween(
            begin: new Offset(0.0, 0.25),
            end: Offset.zero
        );
        
        static Animatable<double> _fastOutSlowInTween = new CurveTween(curve: Curves.fastOutSlowIn);
        static Animatable<double> _easeInTween = new CurveTween(curve: Curves.easeIn);
        
        readonly Animation<Offset> _positionAnimation;
        readonly Animation<double> _opacityAnimation;
        public readonly Widget child;
        
        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: this._positionAnimation,
                child: new FadeTransition(
                    opacity: this._opacityAnimation,
                    child: this.child
                )
            );
        }
    }

    static class _Dialog {
        public static void showDialog(BuildContext context,
            bool barrierDismissible = true, WidgetBuilder builder = null) {
            DialogUtils.showGeneralDialog(
                context: context,
                pageBuilder: (BuildContext buildContext, Animation<double> animation,
                    Animation<double> secondaryAnimation) => {
                    return builder(buildContext);
                },
                barrierDismissible: barrierDismissible,
                barrierLabel: "",
                barrierColor: new Color(0x8A000000),
                transitionDuration: TimeSpan.FromMilliseconds(150),
                transitionBuilder: _buildMaterialDialogTransitions
            );
        }

        static Widget _buildMaterialDialogTransitions(BuildContext context,
            Animation<double> animation, Animation<double> secondaryAnimation, Widget child) {
            return new FadeTransition(
                opacity: new CurvedAnimation(
                    parent: animation,
                    curve: Curves.easeOut
                ),
                child: child
            );
        }
    }
}